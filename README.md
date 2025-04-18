# chatbot-gateway-api

**Gateway API** مبنى بـ .NET 8 (Minimal APIs) يتولى المصادقة، الفوترة، وحدّ المعدل (Rate‑Limiting) ويعمل وسيطًا بين الواجهة والخدمات الخلفية.

## الوظائف الرئيسية
- مصادقة JWT متعددة المستأجرين.
- نقاط نهاية `/files`, `/chat`, `/billing`.
- استدعاء خدمات Python (ingestion, query) عبر HTTP/gRPC.
- جمع Telemetry باستخدام OpenTelemetry.

## المتطلبات
- .NET SDK 8
- Docker (PostgreSQL وChroma)

## الإعداد السريع
```bash
# استنساخ الريبو
git clone https://github.com/ZyadahWorks/chatbot-gateway-api.git
cd chatbot-gateway-api

# تشغيل محلي مع إعادة تحميل تلقائى
dotnet watch run
```

### متغيرات البيئة الأساسية
- `ConnectionStrings__Default`: سلسلة الاتصال بقاعدة بيانات PostgreSQL.
- `Jwt__Secret`: مفتاح توقيع JWT.
- `IngestionService__BaseUrl`: عنوان خدمة Ingestion.

> يمكن ضبطها فى `appsettings.Development.json` أو عبر `.env` فى Docker.

## الاختبارات
```bash
dotnet test
```

## Docker
```bash
docker compose up --build gateway-api
```

## الوثيقة الشاملة
تفاصيل المعمارية موجودة فى `docs/chatbot_project_plan.json`.

---

> هذا الريبو يمثل نقطة الدخول الأولى لمشروع *Specialized Document Chatbot*. يرجى فتح Pull Request لأى إضافة أو إصلاح.


