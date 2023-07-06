Imports Pr = Core.SQL.Parametro
Public Class WsMetodos

    Private _IdMetodo As Integer
    Public ReadOnly Property IdMetodo() As Integer
        Get
            Return _IdMetodo
        End Get
        'Set(ByVal value As Integer)
        '    _IdMetodo = value
        'End Set
    End Property


    Private _Nombre As String
    Public ReadOnly Property Nombre() As String
        Get
            Return _Nombre
        End Get
        'Set(ByVal value As String)
        '    _Nombre = value
        'End Set
    End Property

    Private _Descripcion As String
    Public ReadOnly Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        'Set(ByVal value As String)
        '    _Descripcion = value
        'End Set
    End Property

    Private _Pagineo As Integer
    Public ReadOnly Property Pagineo() As Integer
        Get
            Return _Pagineo
        End Get
        'Set(ByVal value As Integer)
        '    _Pagineo = value
        'End Set
    End Property

    Private _DbError As SQL.DbError
    Public ReadOnly Property DbError() As SQL.DbError
        Get
            Return _DbError
        End Get
    End Property

    Private _BaseDatos As String
    Public ReadOnly Property BaseDatos() As String
        Get
            Return _BaseDatos
        End Get
    End Property


    Public Sub New(IdMetodo)
        Dim DT As New DataTable

        Dim Param As New List(Of Pr)
        Param.Add(New Pr("Opcion", 2, SqlDbType.Int))
        Param.Add(New Pr("IdMetodo", IdMetodo))

        Dim oSql = New SQL
        DT = oSql.DT("Connection", "sp_ws_consulta_cliente_log", Param)

        If DT Is Nothing Then
            Me._IdMetodo = -1
            GoTo Final
        End If

        If DT.Rows.Count = 0 Then
            Me._IdMetodo = -1
            GoTo Final
        End If

        'Mapear Objeto
        Me._IdMetodo = CInt(DT.Rows(0).Item("id_metodo"))
        Me._Nombre = DT.Rows(0).Item("nombre").ToString
        Me._Descripcion = DT.Rows(0).Item("descripcion").ToString
        Me._Pagineo = CInt(DT.Rows(0).Item("pagineo"))
        Me._BaseDatos = oSql.BaseDatos

Final:
        _DbError = oSql.DbErr
    End Sub

End Class
