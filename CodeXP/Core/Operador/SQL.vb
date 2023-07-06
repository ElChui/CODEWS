Public Class SQL

    Public Structure DbError
        Public Codigo As String
        Public Mensaje As String

        Public Sub New(ByVal Cod As String, ByVal Msg As String)
            Codigo = Cod
            Mensaje = Msg
        End Sub
    End Structure

    Private _SQL As clsSQLServer
    Private _BaseDatos As String
    Private _DbError As DbError = New DbError("0", "Ok")

    Public ReadOnly Property BaseDatos() As String
        Get
            Return _BaseDatos
        End Get
    End Property

    Public ReadOnly Property DbErr() As DbError
        Get
            Return _DbError
        End Get
    End Property

    Public Structure Parametro
        Public Nombre As String
        Public Valor As Object
        Public Tipo As SqlDbType

        Public Sub New(ByVal Nombre As String, ByVal Valor As Object, Optional ByVal Tipo As SqlDbType = SqlDbType.VarChar)
            Me.Nombre = Nombre
            Me.Tipo = Tipo
            Me.Valor = Valor
        End Sub
    End Structure

    Public Function GrabaLog(ByVal oWsLog As WsLog) As Boolean
        Dim Result As Integer = 0

        Try
            _SQL = New clsSQLServer("Connection")
            _SQL.Procedimiento = "sp_ws_consulta_cliente_log"
            _SQL.addParam("@i_Opcion", 1)

            'PARAMETROS
            With _SQL
                .addParam("@i_IdMetodo", oWsLog.IdMetodo)
                .addParam("@i_RucCliente", oWsLog.RucCliente)
                .addParam("@i_ClaveConexion", oWsLog.ClaveConexion)
                .addParam("@i_TipoConsulta", oWsLog.TipoConsulta)
                .addParam("@i_Request", oWsLog.Request.InnerXml, SqlDbType.Xml)
                .addParam("@i_Registros", oWsLog.Registros)
                .addParam("@i_Paginas", oWsLog.Paginas)
                .addParam("@i_BaseDatos", oWsLog.BaseDatos)
                .addParam("@i_CodError", oWsLog.CodError)
                .addParam("@i_MsgError", oWsLog.MsgError)
            End With
            Result = _SQL.ExecSP
        Catch ex As Exception
            _DbError = New DbError("SQL", ex.Message)
        End Try

        If Result = 0 Then
            Return True
        End If

        Return False
    End Function

    Public Function DT(ByVal Con As String, ByVal SP As String, ByVal Param As List(Of Parametro)) As DataTable
        Dim Result As New DataTable
        Try
            _SQL = New clsSQLServer(Con)
            _SQL.Procedimiento = SP

            'PARAMETROS
            For Each P As Parametro In Param
                _SQL.addParam("@" + P.Nombre, P.Valor, P.Tipo)
            Next

            'EJECUTAR SQL
            Result = _SQL.ExecSPdt("Datos")

            'RECUPERAR INFROMACION DE SERVIDOR Y BASE DE DATOS
            _BaseDatos = _SQL.BaseDatos
        Catch ex As Exception
            _DbError = New DbError("SQL", ex.Message)
        End Try

        'RESULTADO
        Return Result
    End Function

    Public Function DS(ByVal Con As String, ByVal SP As String, ByVal Param As List(Of Parametro)) As DataSet
        Dim Result As New DataSet
        Try
            _SQL = New clsSQLServer(Con)
            _SQL.Procedimiento = SP

            'PARAMETROS
            For Each P As Parametro In Param
                _SQL.addParam("@" + P.Nombre, P.Valor, P.Tipo)
            Next

            'EJECUTAR SQL
            Result = _SQL.ExecSPds()

            'RECUPERAR INFROMACION DE SERVIDOR Y BASE DE DATOS
            _BaseDatos = _SQL.BaseDatos
        Catch ex As Exception
            _DbError = New DbError("SQL", ex.Message)
        End Try

        'RESULTADO
        Return Result
    End Function

    Public Function ExecSPds(ByVal Con As String, ByVal SP As String, ByVal Param As List(Of Parametro)) As Boolean
        Dim Result As Boolean = True
        Try
            _SQL = New clsSQLServer(Con)
            _SQL.Procedimiento = SP

            'PARAMETROS
            For Each P As Parametro In Param
                _SQL.addParam("@i_" + P.Nombre, P.Valor, P.Tipo)
            Next

            'EJECUTAR SQL
            Result = Not (_SQL.ExecSP)

            'RECUPERAR INFROMACION DE SERVIDOR Y BASE DE DATOS
            _BaseDatos = _SQL.BaseDatos
        Catch ex As Exception
            _DbError = New DbError("SQL", ex.Message)
        End Try

        Return Result
    End Function

End Class
