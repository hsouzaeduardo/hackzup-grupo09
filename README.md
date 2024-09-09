# hackzup-grupo09

![image](https://github.com/user-attachments/assets/f618ecf6-87c5-470e-826f-ec44c514cc2d)


# Documentação do RG-HACKZUP

## 1. Interface de Upload
- **Descrição**: Ponto de entrada para o sistema, onde os arquivos podem ser enviados em diversos formatos, incluindo XML, JSON e CSV.
- **Função**: A interface coleta arquivos de dados que precisam ser processados e inicia o pipeline de tratamento e ingestão.
- **Interação**: Envia os arquivos para o **Azure Blob Storage**.

## 2. Azure Blob Storage
- **Descrição**: Serviço de armazenamento de objetos da Azure, utilizado para armazenar grandes quantidades de dados não estruturados.
- **Função**: Armazena os arquivos que são enviados pelo usuário até que possam ser processados.
- **Interação**: Após armazenar o arquivo, ele é enviado para a **Fila de Arquivos** para dar início ao processamento.

## 3. Fila Arquivos
- **Descrição**: Fila de mensagens responsável por gerenciar o fluxo dos arquivos entre o upload e o processamento do microsserviço.
- **Função**: Reponsável por disparar o evento de arquivo criado para o microsserviço de limpeza de dados.
- **Interação**: Se conecta ao **ms limpeza** para enviar os arquivos.

## 4. Microsserviço de Limpeza (ms limpeza)
- **Descrição**: Microsserviço responsável por realizar a limpeza dos dados, assegurando que estejam no formato correto e sem inconsistências.
- **Função**: Trata os dados, realizando qualquer conversão, formatação ou limpeza necessária antes de serem processados pelo sistema de IA.
- **Interação**: Utiliza o OpenAI (IA) para melhorar a consistência dos dados e preparar para persistência.

## 5. OpenAI
- **Descrição**: Integração com modelos de Inteligência Artificial, como GPT4o, para auxiliar no processo de formatação e padronização dos dados.
- **Função**: Realiza tratamento avançado dos dados, como correções automáticas e normalizações, para garantir que os dados estejam prontos para uso.
- **Interação**: Recebe dados do microsserviço de limpeza e devolve os dados limpos para continuar o processo.

## 6. Microsserviço de Persistência (ms persistência)
- **Descrição**: Microsserviço responsável por salvar os dados já limpos e processados em um banco de dados.
- **Função**: Persistir os dados em um banco de dados relacional para consultas futuras.
- **Interação**: Envia os dados para o banco de dados **PostgreSQL**.

## 7. Banco de Dados (PostgreSQL)
- **Descrição**: Banco de dados relacional que armazena os dados processados.
- **Função**: Serve como repositório para dados persistidos após a etapa de processamento e limpeza.
- **Interação**: Permite consultas via API sobre os dados de famílias, cidades, idades, etc.

## 8. API de Consulta
- **Descrição**: Interface API que permite a consulta dos dados armazenados no banco de dados.
- **Função**: Disponibiliza endpoints para consulta de informações sobre famílias, cidades, idades e filhos.
- **Interação**: Conecta-se ao banco de dados PostgreSQL para obter as informações solicitadas.

## 9. Monitoramento (Monitor, Log Analytics, App Insights)
- **Descrição**: Conjunto de ferramentas para monitoramento e análise do sistema em tempo real.
- **Função**: Monitorar o desempenho dos microsserviços e da API, além

