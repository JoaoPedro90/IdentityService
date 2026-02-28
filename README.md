# 🌱 Plataforma Agro - MVP (Microserviços)

Este projeto implementa um **MVP (Minimum Viable Product)** de uma plataforma para monitoramento agrícola, utilizando arquitetura de **microserviços orientada a eventos**.

A solução permite:
- Autenticação de produtores
- Cadastro de propriedades e talhões
- Ingestão de dados de sensores
- Geração de alertas automáticos

---

# 🧱 Arquitetura da Solução

A solução é composta por quatro microserviços principais:

- **IdentityService** → Autenticação e geração de JWT  
- **PropertyService** → Gestão de propriedades e talhões  
- **SensorService** → Recebimento e processamento de dados de sensores  
- **AlertService** → Geração de alertas  

## 📊 Diagrama da Arquitetura

![Arquitetura](./docs/arquitetura.png)

---

## 🔄 Comunicação entre serviços

A comunicação é feita de duas formas:

### 🔹 Síncrona (HTTP REST)
Utilizada para operações que precisam de resposta imediata:
- Login
- Cadastro
- Consulta de dados

### 🔹 Assíncrona (RabbitMQ)
Utilizada para eventos de domínio:

- `talhao.created` → Property → Sensor  
- `sensor.reading.created` → Sensor → Alert  

---

## 🧩 Tecnologias utilizadas

- .NET 8
- ASP.NET Core
- RabbitMQ
- SQL Server
- Docker / Docker Compose
- GitHub Actions (CI/CD)
- Prometheus + Grafana (Observabilidade)

---

# ⚙️ Justificativa técnica das decisões arquiteturais

## 1. Microserviços

A aplicação foi dividida em múltiplos serviços independentes.

### Benefícios:
- Separação de responsabilidades
- Escalabilidade independente
- Deploy isolado
- Manutenção facilitada

---

## 2. Comunicação via eventos (RabbitMQ)

Eventos são utilizados para comunicação entre serviços.

### Benefícios:
- Desacoplamento
- Resiliência
- Escalabilidade
- Extensibilidade

---

## 3. Banco de dados por serviço

Cada serviço possui seu próprio banco:

- Identity → AuthDB  
- Property → PropriedadeDB  
- Sensor → SensorDB  
- Alert → AlertsDB  

### Benefícios:
- Independência entre serviços
- Evita conflitos de schema
- Permite otimizações específicas

---

## 4. JWT para autenticação

O IdentityService emite tokens JWT.

### Benefícios:
- Stateless
- Baixa latência
- Segurança
- Facilidade de integração

---

## 5. Docker

Todos os serviços são executados em containers.

### Benefícios:
- Ambiente padronizado
- Fácil deploy
- Isolamento de dependências

---

## 6. CI/CD automatizado

Cada serviço possui pipeline via GitHub Actions.

### Benefícios:
- Build automatizado
- Execução de testes
- Redução de erros manuais
- Integração contínua

---

# 🛡️ Como os requisitos não funcionais são atendidos

## 🔐 Segurança

- Autenticação via JWT
- Validação de token em todos os serviços
- Variáveis de ambiente para segredos

---

## 📈 Escalabilidade

- Arquitetura de microserviços
- Comunicação assíncrona com RabbitMQ
- Possibilidade de múltiplas instâncias

---

## 🔄 Disponibilidade e Resiliência

- RabbitMQ armazena mensagens
- Serviços independentes
- Health checks

---

## ⚡ Performance

- Processamento assíncrono
- Redução de chamadas síncronas
- Banco isolado por serviço

---

## 👀 Observabilidade

Implementada com:

- Prometheus (coleta de métricas)
- Grafana (dashboards)

Cada serviço expõe:

- `/health`
- `/metrics`

---

## 🧩 Manutenibilidade

- Clean Architecture
- Separação em camadas
- Baixo acoplamento

---

## 🔁 Consistência

- Consistência eventual
- Uso de eventos para sincronização

---

## 🔒 Confiabilidade

- Retry em mensagens
- Idempotência
- Uso de filas

---

# 🚀 Como rodar o projeto

## Pré-requisitos

- Docker
- Docker Compose

---

## Subir a aplicação

```bash
docker compose up -d --build

## 🔗 Microsserviços Relacionados

- 🔐 **IdentityService**  
  Serviço responsável por autenticação e autorização.  
  👉 https://github.com/JoaoPedro90/IdentityService.git  

- 🏠 **PropertyService**  
  Serviço responsável pelo gerenciamento de propriedades.  
  👉 https://github.com/JoaoPedro90/PropertyService.git  

- 📡 **SensorService**  
  Serviço responsável pelo gerenciamento de sensores.  
  👉 https://github.com/JoaoPedro90/SensorService.git  
