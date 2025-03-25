# 📄 CV Management API

Et sikkert REST API for å opprette, hente og administrere CV-er med roller og autentisering (JWT). Admin-bruker kan opprette andre brukere og gi tilgang.

---

## Teknologi

- ASP.NET Core 8
- MySQL (Pomelo)
- Entity Framework Core
- Identity + JWT Autentisering
- Swagger UI

---

## Setup (for utviklere)

### 1. Klon repoet og åpne prosjektet
```bash
git clone <repo-url>
cd CvManagementApi
```

### 2. Konfigurer `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=cv_management;User=root;Password=;"
},
"Jwt": {
  "Key": "SuperSecretKeyThatIsLongEnoughToBeValid!!",
  "Issuer": "https://localhost:5001",
  "Audience": "https://localhost:5001"
}
```

> Pass på at JWT-key er minst **32 tegn**

---

## Installer avhengigheter

```bash
dotnet restore
```

---

### 3. Opprett database

```bash
dotnet ef database update
```

> Dette kjører migrasjonene og seed-er inn én Admin og én testbruker med tilhørende CV-er

---

## Testing

### Start API-et

```bash
dotnet run
```

### Swagger

Gå til:  
[http://localhost:5001/swagger](http://localhost:5001/swagger)

---

## Logg inn

```bash
curl -X POST http://localhost:5001/api/user/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin", "password":"Admin2025!"}'
```

> Du får tilbake en `token`.

---

## Bruk token i videre API-kall

```bash
curl -H "Authorization: Bearer <fjern tegnene og plasser token rett etter bearer>" http://localhost:5001/api/cv/-1
```

---

## Brukerroller

- Admin: Har tilgang til alle brukere og CV-er
- User: Kan kun se/endre egne CV-er

---

## Standardbrukere

| Rolle | Brukernavn | Passord     |
|-------|------------|-------------|
| Admin | `admin`    | `Admin2025!` |

---

## Logg inn på databasen (MySQL)

```bash
mysql -u root
USE cv_management;

-- Se alle brukere:
SELECT Id, UserName, Role FROM Users;

-- Se alle CV-er:
SELECT * FROM CVs;
```

---

## Mapper

- `Controllers/`: API-endepunkter
- `Models/`: Entity-modeller (User, CV, osv.)
- `Services/`: JWT-generering
- `Data/`: AppDbContext og seed-data
- `Migrations/`: EF Core migrasjoner

---

<<<<<<< HEAD
## Kontakt

Backend utviklere 
=======
## 📬 Kontakt

Backend utviklere 
>>>>>>> 944b764 (Siste oppdatering – full push av Kickstep-prosjektet)
