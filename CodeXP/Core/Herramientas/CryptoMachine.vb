Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.Cryptography
Imports System.Text

Friend Module CryptoMachine

#Region " Método de Cifrado y Reversión de Cifrado de Datos "

    Public Function EncryptData(ByVal Data As Object) As Byte()
        Dim cryptoData() As Byte

        ' Verificar que el dato no es nulo
        If Data Is Nothing Then Return Nothing

        Try
            ' Serializar el objeto en forma binaria
            Dim rawData() As Byte = SerializeObject(Data)

            ' Cifrar el objeto serializado
            cryptoData = EncryptBinaryData(rawData)
        Catch
            Return Nothing
        End Try

        Return cryptoData
    End Function

    Public Function DecryptData(ByVal Data() As Byte) As Object
        Dim oResult As Object

        Try
            ' Verificar que el dato no es nulo ni vacio
            If Data Is Nothing Then Return Nothing

            ' Reversar el algoritmo sobre la cadena
            Dim cryptoData() As Byte = CType(Data.Clone(), Byte())
            Dim rawData() As Byte = DecryptBinaryData(cryptoData)

            ' Deserializar el objeto recuperado
            oResult = DeserializeObject(rawData)
        Catch
            Return Nothing
        End Try

        Return oResult
    End Function

#End Region

#Region " Método de Cifrado y Reversión de Cifrado de Datos "

    Public Function EncryptString(ByVal Data As String) As Byte()
        Dim cryptoData() As Byte

        ' Verificar que el dato no es nulo
        If Data Is Nothing OrElse Data.Length = 0 Then Return Nothing

        Try
            ' Obtener los bytes del string
            Dim rawData() As Byte = Encoding.Default.GetBytes(Data)

            ' Cifrar el objeto serializado
            cryptoData = EncryptBinaryData(rawData)
        Catch
            Return Nothing
        End Try

        Return cryptoData
    End Function

    Public Function DecryptString(ByVal Data() As Byte) As String
        Dim oResult As String

        Try
            ' Verificar que el dato no es nulo ni vacio
            If Data Is Nothing Then Return Nothing

            ' Reversar el algoritmo sobre la cadena
            Dim cryptoData() As Byte = CType(Data.Clone(), Byte())
            Dim rawData() As Byte = DecryptBinaryData(cryptoData)

            ' Deserializar el objeto recuperado
            oResult = Encoding.Default.GetString(rawData)
        Catch
            Return Nothing
        End Try

        Return oResult
    End Function
#End Region

#Region " Método para serializar un objeto en formato binario "

    Private Function SerializeObject(ByVal value As Object) As Byte()
        Dim rawData() As Byte
        Dim formatter As New BinaryFormatter

        Dim memoStream As New MemoryStream
        formatter.Serialize(memoStream, value)
        rawData = memoStream.GetBuffer()
        memoStream.Close()

        Return rawData
    End Function

#End Region

#Region " Método para de-serializar el objeto desde formato binario "

    Private Function DeserializeObject(ByVal value() As Byte) As Object
        Dim formatter As New BinaryFormatter
        Dim memoStream As New MemoryStream(value)
        Dim result As Object = formatter.Deserialize(memoStream)
        memoStream.Close()
        Return result
    End Function

#End Region

#Region " Aplicación del algoritmo de cifrado sobre los datos "
    Private Function EncryptBinaryData(ByVal Data() As Byte) As Byte()
        ' Obtener el algoritmo del Cifrado a utilizar
        Dim myRijndael As New RijndaelManaged

        ' Crear el buffer que servirá como salida de datos
        Dim outData As New MemoryStream

        ' Generar una nueva Clave y guardarla en el buffer de salida
        myRijndael.GenerateKey()
        Dim key() As Byte = myRijndael.Key

        outData.WriteByte(CType(key.Length, Byte))
        outData.Write(key, 0, key.Length)

        ' Generar un nuevo vector de inicialización
        myRijndael.GenerateIV()
        Dim iv() As Byte = myRijndael.IV

        outData.WriteByte(CType(iv.Length, Byte))
        outData.Write(iv, 0, iv.Length)

        ' Obtener un cifrador
        Dim encryptor As ICryptoTransform = myRijndael.CreateEncryptor(key, iv)

        ' Cifrar la data
        Dim csEncrypt As New CryptoStream(outData, encryptor, CryptoStreamMode.Write)
        csEncrypt.Write(Data, 0, Data.Length)
        csEncrypt.FlushFinalBlock()

        ' Retornar los datos cifrados
        Return outData.ToArray()
    End Function
#End Region

#Region " Aplicación del algoritmo de descifrado sobre los datos "
    Private Function DecryptBinaryData(ByVal Data() As Byte) As Byte()
        ' Crear el buffer que servirá como entrada de datos
        Dim inData As New MemoryStream(Data)

        ' Obtener el algoritmo del Cifrado a utilizar
        Dim myRijndael As New RijndaelManaged

        ' Recuperar la Clave y guardarla en el buffer de salida
        Dim keySize As Integer = inData.ReadByte()
        Dim key(keySize - 1) As Byte
        inData.Read(key, 0, keySize)

        ' Recuperar el Vector de Inicialización
        Dim ivSize As Integer = inData.ReadByte()
        Dim iv(ivSize - 1) As Byte
        inData.Read(iv, 0, ivSize)

        ' Obtener un descifrador
        Dim decryptor As ICryptoTransform = myRijndael.CreateDecryptor(key, iv)

        ' Descifrar la data
        Dim rData(Data.Length - 1) As Byte
        Dim csDecrypt As New CryptoStream(inData, decryptor, CryptoStreamMode.Read)
        Dim readBytes As Integer = csDecrypt.Read(rData, 0, Data.Length)

        ' Retornar los datos descifrados
        Dim rawData(readBytes - 1) As Byte
        Array.Copy(rData, rawData, readBytes)
        Return rawData
    End Function
#End Region

End Module
