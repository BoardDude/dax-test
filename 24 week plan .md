## 24-week plan: Profitability Portal & Bloomberg Reporting (React, C#/.NET, SQL, APIs, testing, deployment)

### Goal
- Build and modernize the Profitability Portal and deliver Bloomberg reporting capabilities:
  - Frontend modernization to React
  - API service performance improvements
  - Bloomberg reporting portal: subscriptions added/removed, per‑user costs
  - Request/authorize workflow for data requests
  - User SID migration tooling and process
  - Automated weekly/monthly subscription reports per user
  - Base entitlements by role (e.g., sales trader: Euronext L1, LSE L2)

### Overall cadence
- Weekly: 1–2 modules + mini‑deliverable tied to the portal/reporting
- Monthly: demo of working functionality and technical understanding
- Ongoing: capture notes, questions, and follow‑ups; weekly check‑ins

### Weeks 1–4: Foundations aligned to portal data model and API
- [ ] Week 1
  - [ ] C#: syntax, types, collections, LINQ
  - [ ] Git: branching, PRs
  - [ ] Deliverable: web API that models Bloomberg subscriptions, users, and costs with unit tests
- [ ] Week 2
  - [ ] OOP: classes, objects, interfaces, SOLID
  - [ ] Error handling, disposables (IDisposable, using)
  - [ ] Deliverable: refactor domain model for subscriptions, costs, and entitlements; define interfaces
- [ ] Week 3
  - [ ] SQL: schema design for users, entitlements, subscriptions, costs, audit
  - [ ] Tools: local SQL Server or container
  - [ ] Deliverable: DB schema + DDL scripts; realistic seed data for Bloomberg scenarios
- [ ] Week 4
  - [ ] Entity Framework Core: DbContext, migrations, CRUD
  - [ ] Deliverable: data access layer for portal entities via EF Core
  - [ ] Monthly demo: domain + DB ready for portal

### Weeks 5–8: Patterns, DI, async, logging for API performance
- [ ] Week 5
  - [ ] Dependency Injection (built-in container), lifetimes
  - [ ] Deliverable: register services for subscription, cost, entitlement domains; DI throughout
- [ ] Week 6
  - [ ] Factory pattern for report generators (subscriptions, costs, entitlements)
  - [ ] Producer/consumer for report jobs (Channels/Queues)
  - [ ] Deliverable: background job service generating reports efficiently
- [ ] Week 7
  - [ ] Async programming: Tasks, async/await, pitfalls (deadlocks, sync context)
  - [ ] Deliverable: async API/service methods with cancellation/timeouts for long queries
- [ ] Week 8
  - [ ] Logging: structured logs; request/response correlation; write logs to DB
  - [ ] Deliverable: centralized logging with correlation IDs; performance timings
  - [ ] Monthly demo: API perf baseline with observability

### Weeks 9–12: Web APIs and automated testing for portal features
- [ ] Week 9
  - [ ] ASP.NET Core controllers/minimal APIs, routing, middleware
  - [ ] Deliverable: API endpoints for users, subscriptions, entitlements, costs; health checks
- [ ] Week 10
  - [ ] Validation, configuration, options pattern; error handling middleware
  - [ ] Deliverable: robust validation for request/authorize workflow; problem details responses
- [ ] Week 11
  - [ ] Testing: unit, integration (WebApplicationFactory), API endpoint tests
  - [ ] Deliverable: test suite covering report generation and API contracts
- [ ] Week 12
  - [ ] DB integration tests; test containers; seeding and teardown
  - [ ] Deliverable: integration tests hitting a real DB
  - [ ] Monthly demo: end‑to‑end API + tests for portal basics

### Weeks 13–16: SQL mastery and performance for reporting
- [ ] Week 13
  - [ ] Advanced joins (OUTER), CROSS APPLY, set ops
  - [ ] Deliverable: tuned queries for subscriptions added/removed per user/timeframe
- [ ] Week 14
  - [ ] Window functions: OVER/PARTITION BY; ranking, aggregates
  - [ ] Deliverable: per‑user cost aggregation and trends using window functions
- [ ] Week 15
  - [ ] Indexing strategies, execution plans, parameter sniffing
  - [ ] Deliverable: performance analysis for core report queries; scripts + docs
- [ ] Week 16
  - [ ] Long‑running query investigation, tooling (e.g., DMVs), EF performance tips
  - [ ] Deliverable: performance checklist + tuned queries powering the portal
  - [ ] Monthly demo: reporting performance improvements

### Weeks 17–20: Full web app build for the Profitability Portal
- [ ] Week 17
  - [ ] Domain model + use cases; clean boundaries; DTOs vs entities
  - [ ] Deliverable: portal skeleton (React frontend + backend) with layers and DI
- [ ] Week 18
  - [ ] Feature verticals: endpoints + EF + validations
  - [ ] Deliverable: request/authorize workflow; base entitlements by role; React pages/forms; tested end‑to‑end
- [ ] Week 19
  - [ ] Observability: logging, metrics, correlation IDs
  - [ ] Deliverable: metrics for report latency, job throughput, API SLIs/SLOs
- [ ] Week 20
  - [ ] Hardening: auth, rate limiting, input hardening
  - [ ] Deliverable: resilience policies (retry, circuit breaker) for report jobs and APIs
  - [ ] Monthly demo: working portal features + resilience

### Weeks 21–24: Finalization, deployment, capstone demo
- [ ] Week 21
  - [ ] Finish features; handle edge cases; improve DX (scripts)
  - [ ] Deliverable: polished API with docs (OpenAPI) for portal/reporting
- [ ] Week 22
  - [ ] Deployment: containerization; local compose; basic CI workflow
  - [ ] Deliverable: build/test pipeline; dockerized portal (React) + API + DB
- [ ] Week 23
  - [ ] Regression tests, load smoke, data migrations
  - [ ] Deliverable: load tests for report generation; SID migration scripts and runbook
- [ ] Week 24
  - [ ] Final demo: modernized frontend, performant APIs, reporting, workflows, deployment
  - [ ] Deliverable: README with setup, architecture diagram, and decisions

### Checkpoints
- [ ] Monthly demos at Weeks 4, 8, 12, 16, 20, 24
- [ ] Weekly check‑ins with a short status, risks, next goals

