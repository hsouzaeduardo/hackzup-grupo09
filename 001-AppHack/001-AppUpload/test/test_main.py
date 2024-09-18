import unittest
from unittest.mock import MagicMock
from unittest.mock import MagicMock, patch
from main import upload_to_blob, send_message_to_service_bus, get_file_type

class TestSendMessageToServiceBus(unittest.TestCase):
    def test_send_message_success(self):
        file_name = "test_file.json"
        file_type = "json"
        expected_result = "Mensagem enviada ao Service Bus com sucesso!"
        
        result = send_message_to_service_bus(file_name, file_type)
        
        self.assertEqual(result, expected_result)
    
    def test_send_message_failure(self):
        file_name = "test_file.csv"
        file_type = "csv"
        expected_result = "Erro ao enviar a mensagem: ConnectionError"
        
        result = send_message_to_service_bus(file_name, file_type)
        
        self.assertEqual(result, expected_result)

        class TestSendMessageToServiceBus(unittest.TestCase):
            def test_send_message_success(self):
                file_name = "test_file.json"
                file_type = "json"
                expected_result = "Mensagem enviada ao Service Bus com sucesso!"
                
                result = send_message_to_service_bus(file_name, file_type)
                
                self.assertEqual(result, expected_result)
            
            def test_send_message_failure(self):
                file_name = "test_file.csv"
                file_type = "csv"
                expected_result = "Erro ao enviar a mensagem: ConnectionError"
                
                result = send_message_to_service_bus(file_name, file_type)
                
                self.assertEqual(result, expected_result)

        class TestGetFileType(unittest.TestCase):
            def test_get_file_type_image(self):
                file = MagicMock()
                file.type = "image/jpeg"
                self.assertEqual(get_file_type(file), "image")

            def test_get_file_type_json(self):
                file = MagicMock()
                file.type = "application/json"
                self.assertEqual(get_file_type(file), "json")

            def test_get_file_type_csv(self):
                file = MagicMock()
                file.type = "text/csv"
                self.assertEqual(get_file_type(file), "csv")

            def test_get_file_type_unknown(self):
                file = MagicMock()
                file.type = "application/xml"
                self.assertEqual(get_file_type(file), "unknown")

        if __name__ == '__main__':
            unittest.main()

            class TestSendMessageToServiceBus(unittest.TestCase):
                def test_send_message_success(self):
                    file_name = "test_file.json"
                    file_type = "json"
                    expected_result = "Mensagem enviada ao Service Bus com sucesso!"
                    
                    result = send_message_to_service_bus(file_name, file_type)
                    
                    self.assertEqual(result, expected_result)
                
                def test_send_message_failure(self):
                    file_name = "test_file.csv"
                    file_type = "csv"
                    expected_result = "Erro ao enviar a mensagem: ConnectionError"
                    
                    result = send_message_to_service_bus(file_name, file_type)
                    
                    self.assertEqual(result, expected_result)

            class TestGetFileType(unittest.TestCase):
                def test_get_file_type_image(self):
                    file = MagicMock()
                    file.type = "image/jpeg"
                    self.assertEqual(get_file_type(file), "image")

                def test_get_file_type_json(self):
                    file = MagicMock()
                    file.type = "application/json"
                    self.assertEqual(get_file_type(file), "json")

                def test_get_file_type_csv(self):
                    file = MagicMock()
                    file.type = "text/csv"
                    self.assertEqual(get_file_type(file), "csv")

                def test_get_file_type_unknown(self):
                    file = MagicMock()
                    file.type = "application/xml"
                    self.assertEqual(get_file_type(file), "unknown")

            class TestUploadToBlob(unittest.TestCase):
                @patch('main.BlobServiceClient')
                def test_upload_to_blob_success(self, MockBlobServiceClient):
                    file = MagicMock()
                    file.name = "test_file.txt"
                    mock_blob_client = MockBlobServiceClient().get_blob_client()
                    mock_blob_client.upload_blob.return_value = None
                    
                    result = upload_to_blob(file)
                    
                    self.assertEqual(result, f"Arquivo {file.name} enviado com sucesso!")
                    mock_blob_client.upload_blob.assert_called_once_with(file)

                @patch('main.BlobServiceClient')
                def test_upload_to_blob_failure(self, MockBlobServiceClient):
                    file = MagicMock()
                    file.name = "test_file.txt"
                    mock_blob_client = MockBlobServiceClient().get_blob_client()
                    mock_blob_client.upload_blob.side_effect = Exception("Upload failed")
                    
                    result = upload_to_blob(file)
                    
                    self.assertTrue("Erro ao enviar o arquivo: Upload failed" in result)
                    mock_blob_client.upload_blob.assert_called_once_with(file)

            if __name__ == '__main__':
                unittest.main()

if __name__ == '__main__':
    unittest.main()