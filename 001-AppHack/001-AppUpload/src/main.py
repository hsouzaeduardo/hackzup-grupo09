import streamlit as st
from azure.storage.blob import BlobServiceClient
from azure.servicebus import ServiceBusClient, ServiceBusMessage
import os
import json

# Configurar as credenciais do Storage Account e do Service Bus
STORAGE_ACCOUNT_NAME = st.secrets["STORAGE_ACCOUNT_NAME"]
STORAGE_ACCOUNT_KEY = st.secrets["STORAGE_ACCOUNT_KEY"]
CONTAINER_NAME = st.secrets["CONTAINER_NAME"]

SERVICE_BUS_CONNECTION_STR = st.secrets["SERVICE_BUS_CONNECTION_STR"]
SERVICE_BUS_QUEUE_NAME = st.secrets["SERVICE_BUS_QUEUE_NAME"]

# Rest of your code remains the same
# ...

# Função para fazer o upload do arquivo para o Azure Storage Account
def upload_to_blob(file):
    try:
        blob_service_client = BlobServiceClient(
            account_url=f"https://{STORAGE_ACCOUNT_NAME}.blob.core.windows.net",
            credential=STORAGE_ACCOUNT_KEY
        )
        blob_client = blob_service_client.get_blob_client(container=CONTAINER_NAME, blob=file.name)
        blob_client.upload_blob(file)
        return f"Arquivo {file.name} enviado com sucesso!"
    except Exception as e:
        return f"Erro ao enviar o arquivo: {e}"

# Função para determinar o tipo do arquivo
def get_file_type(file):
    if file.type.startswith("image"):
        return "image"
    elif file.type == "application/json":
        return "json"
    elif file.type == "text/csv":
        return "csv"
    else:
        return "unknown"

# Função para enviar uma mensagem ao Azure Service Bus com JSON
def send_message_to_service_bus(file_name, file_type):
    try:
        # Criar o conteúdo da mensagem em formato JSON
        message_content = {
            "file_name": file_name,
            "file_type": file_type
        }

        # Converter o dicionário em uma string JSON
        message_json = json.dumps(message_content)

        # Conectar ao Service Bus e enviar a mensagem
        servicebus_client = ServiceBusClient.from_connection_string(SERVICE_BUS_CONNECTION_STR)
        with servicebus_client:
            sender = servicebus_client.get_queue_sender(queue_name=SERVICE_BUS_QUEUE_NAME)
            with sender:
                message = ServiceBusMessage(message_json)
                sender.send_messages(message)
        
        return "Mensagem enviada ao Service Bus com sucesso!"
    except Exception as e:
        return f"Erro ao enviar a mensagem: {e}"

st.title("Grupo 9 App de Upload de Arquivos")

st.sidebar.image(
    "https://hackathonzup940f.blob.core.windows.net/logo/logo-removebg-preview.png",
    use_column_width=True
)

uploaded_file = st.file_uploader("Escolha um arquivo", type=["csv", "json", "jpg", "png"])

if uploaded_file is not None:
    # Fazer o upload para o Storage Account
    upload_message = upload_to_blob(uploaded_file)
    st.write(upload_message)
    
    if "sucesso" in upload_message:
        # Determinar o tipo do arquivo
        file_type = get_file_type(uploaded_file)
        
        # Enviar mensagem para o Service Bus com JSON
        service_bus_message = send_message_to_service_bus(uploaded_file.name, file_type)
        st.write(service_bus_message)
