# MVP – Plataforma Agro (IdentityService, PropertyService, SensorService, AlertService)

Este repositório contém um MVP de uma plataforma baseada em microserviços para gestão de propriedades/talhões, ingestão de sensores simulados e geração de alertas.

## Sumário
- [Visão do MVP](#visão-do-mvp)
- [Serviços](#serviços)
- [Arquitetura da Solução](#arquitetura-da-solução)
- [Mensageria (RabbitMQ)](#mensageria-rabbitmq)
- [Decisões Arquiteturais e Justificativas](#decisões-arquiteturais-e-justificativas)
- [Requisitos Não Funcionais](#requisitos-não-funcionais)
- [Como Rodar Localmente](#como-rodar-localmente)
- [Fluxos do MVP](#fluxos-do-mvp)
- [Estrutura Recomendada de Pastas](#estrutura-recomendada-de-pastas)
- [Evoluções Futuras](#evoluções-futuras)

---

## Visão do MVP

### Objetivo
Entregar o fluxo mínimo que comprova valor:

1. Produtor realiza **login** no **IdentityService** e recebe **JWT**.
2. Produtor cadastra **Propriedade** e **Talhões** no **PropertyService**.
3. Ao criar um talhão, o PropertyService **publica um evento** `talhao.created` no RabbitMQ.
4. O **SensorService** **consome** `talhao.created` e registra o talhão como “conhecido”.
5. O SensorService expõe API para **ingestão de dados de sensores simulados** (umidade, temperatura, precipitação) para um `talhaoId`.
6. O SensorService publica `sensor.reading.recorded` e o **AlertService** consome para **gerar alertas**.

---

## Serviços

### 1) IdentityService
- Cadastro/login do produtor (MVP pode começar com login).
- Emissão de **JWT** (claims: `userId`, `email`, etc).
- Banco próprio (**IdentityDb**).

### 2) PropertyService
- CRUD de Propriedade e Talhão.
- Publicação de evento de domínio `talhao.created` no RabbitMQ.
- Banco próprio (**PropertyDb**).

### 3) SensorService
- Mantém um catálogo local mínimo de talhões conhecidos (apenas o necessário para validar ingestão).
- Endpoint para ingestão de leituras simuladas.
- Publica `sensor.reading.recorded`.
- Banco próprio (**SensorDb**).

### 4) AlertService
- Consome `sensor.reading.recorded`.
- Aplica regras simples (threshold) e gera alertas.
- API para consulta de alertas.
- Banco próprio (**AlertDb**).

---

## Arquitetura da Solução

### Diagrama (Arquitetura MVP)

> O diagrama abaixo usa *event-driven* para desacoplar criação de talhões e ingestão/alertas.

```mermaid
flowchart LR
  U[Cliente / Front-end / Postman] -->|Login| ID[IdentityService]
  U -->|CRUD Propriedade/Talhão (JWT)| PS[PropertyService]
  U -->|Enviar leituras (JWT)| SS[SensorService]
  U -->|Consultar alertas (JWT)| AS[AlertService]

  subgraph MQ[RabbitMQ]
    EX1[(Exchange: property.events)]
    EX2[(Exchange: sensor.events)]
  end

  PS -->|publish talhao.created| EX1
  SS -->|consume talhao.created| EX1

  SS -->|publish sensor.reading.recorded| EX2
  AS -->|consume sensor.reading.recorded| EX2

  ID --> DB1[(DB Identity)]
  PS --> DB2[(DB Property)]
  SS --> DB3[(DB Sensor)]
  AS --> DB4[(DB Alert)]
