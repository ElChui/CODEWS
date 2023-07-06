Imports System.Xml
Imports System.Text

Partial Public Class Util

    Public Function DtXml(ByVal dt As DataTable, ByVal NombreRaiz As String, Optional ByVal NombreHijo As String = "Detalle") As XmlDocument
        Dim XmlReturn As New XmlDocument
        XmlReturn.LoadXml(ConvertDataTableToXML(dt, NombreRaiz, NombreHijo))
        Return XmlReturn
    End Function

    Private Function ConvertDataTableToXML(ByVal tableToExport As DataTable, ByVal NombreRaiz As String, ByVal NombreHijo As String) As String
        Dim formattedXML As New StringBuilder
        Dim DocXml As New XmlDocument
        Dim node As XmlNode = DocXml.CreateNode(XmlNodeType.Element, String.Empty, NombreRaiz, Nothing)

        Dim dtColumns As DataColumnCollection = tableToExport.Columns

        For Each dataItem As DataRow In tableToExport.Rows
            Dim element As XmlElement = DocXml.CreateElement(NombreHijo)
            For Each Column As DataColumn In dtColumns
                Dim value As Object = dataItem(Column)
                Dim tmpElemXML As XmlElement = DocXml.CreateElement(Column.ColumnName)
                If Not value Is Nothing Then
                    tmpElemXML.InnerXml = value.ToString
                Else
                    tmpElemXML.InnerXml = String.Empty
                End If

                element.AppendChild(tmpElemXML)
            Next
            node.AppendChild(element)
        Next

        DocXml.AppendChild(node)

        Return DocXml.InnerXml
    End Function

End Class

