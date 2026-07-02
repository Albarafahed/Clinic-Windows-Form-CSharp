# 🏥 Clinic Management System

![C#](https://img.shields.io/badge/C%23-.NET-blue)
![Framework](https://img.shields.io/badge/.NET-Framework%204.7.2-purple)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)
![Architecture](https://img.shields.io/badge/Architecture-3--Tier-success)
![WinForms](https://img.shields.io/badge/UI-WinForms-orange)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

> A complete Desktop Clinic Management System built with **C# WinForms**, **SQL Server**, and **3-Tier Architecture**, covering the entire patient journey from registration to diagnosis, pharmacy, billing, and administration.

![Dashboard](Image/frmMain1.png)

---

# 📋 Overview

Clinic Management System is a complete desktop solution designed to automate daily clinical operations inside small and medium healthcare centers.

The system manages:

- Patients
- Doctors
- Appointments
- Medical Visits
- Nurses
- Pharmacy
- Cashier
- Medical Services
- Users & Permissions

The application follows a clean **3-Tier Architecture**, separating the Presentation Layer, Business Logic Layer, and Data Access Layer to make the project scalable, maintainable, and easy to extend.

---

# ✨ Main Features

## 👤 Patient Management

- Add / Edit Patients
- Search Patients
- View Patient Profile
- Medical History
- Prevent Duplicate Records
- Validation Rules

---

## 👨‍⚕️ Doctor Management

- Add / Edit Doctors
- Multiple Specializations
- Working Days
- Working Hours
- Consultation Fees
- License Number
- Doctor Availability

---

## 📅 Appointment Management

- Appointment Scheduling
- Appointment Types
- Queue Management
- Reschedule Appointments
- Check-In Process
- Prevent Double Booking

---

## 🩺 Medical Visit Management

- Start Visit
- Diagnosis
- Clinical Notes
- Prescriptions
- Medicines
- Visit Status

---

## 💊 Pharmacy

- Prescription Processing
- Pending Prescriptions
- Direct Sales
- Drug Management
- Medicine Dispensing

---

## 💳 Cashier

- Issue Invoices
- Process Payments
- Medical Bills
- Medicine Returns
- Pending Prescription Editing

---

## 👩‍⚕️ Nurse Station

- Record Vital Signs
- Prepare Patient Before Doctor Visit

---

## 👥 User Management

- Login System
- Password Management
- User Accounts
- Roles & Permissions

---

## 🏥 Medical Services

- Medical Service Types
- Appointment Types
- Service Configuration

---

# 🔄 Complete Patient Workflow

The system follows the complete clinical workflow:

```text
Patient Registration
        │
        ▼
Reception
(Appointment Booking)
        │
        ▼
Cashier
(Check-In Payment)
        │
        ▼
Nurse Station
(Vital Signs)
        │
        ▼
Doctor Station
(Diagnosis + Prescription)
        │
        ▼
Pharmacy
(Dispense Medicines)
        │
        ▼
Cashier
(Final Payment & Invoice)
```

---

# 🏗 System Architecture

The project follows the **3-Tier Architecture** pattern.

```
Presentation Layer
        │
        ▼
Business Logic Layer
        │
        ▼
Data Access Layer
        │
        ▼
SQL Server Database
```

### Presentation Layer

Responsible for:

- WinForms UI
- User Controls
- User Interaction
- Input Validation

Project:

```
Clinic
```

---

### Business Layer

Responsible for:

- Business Rules
- Validation
- Object Management
- Workflow Logic

Project:

```
Clinic_Business
```

---

### Data Access Layer

Responsible for:

- SQL Server
- Stored Procedures
- Transactions
- CRUD Operations
- Bulk Insert
- Logging

Project:

```
Clinic_DataAccess
```

---

# 📂 Project Structure

```text
Clinic_System
│
├── Clinic
│   ├── Login
│   ├── Person
│   ├── Patient
│   ├── Doctor
│   ├── User
│   ├── Medical Services
│   ├── ControlsMain
│   └── Resources
│
├── Clinic_Business
│
├── Clinic_DataAccess
│
└── Image
```

---

# 🖼 Database Diagram

![Database](Image/DB/DBClinc6.png)

# 📸 System Screenshots

---

# 🔐 Login

| Screen | Preview |
|---------|---------|
| Login | ![](Image/Login/frmLogin.png) |

---

# 👥 Person Management

Manage all people in the system including patients, doctors, and users.

| Screen | Preview |
|---------|---------|
| People List | ![](Image/Person/frmlistPeople.png) |
| Add / Update Person | ![](Image/Person/frmaddUpdateperson.png) |
| Person Details | ![](Image/Person/frmShowPerson.png) |
| Trash | ![](Image/Person/frmTrashPeople.png) |

---

# 🧑 Patient Management

Complete patient management with search, validation, and medical information.

| Screen | Preview |
|---------|---------|
| Patient List | ![](Image/Patient/frmListPatients.png) |
| Add / Update Patient | ![](Image/Patient/frmAddUpdatePatient.png) |
| Patient Details | ![](Image/Patient/frmShowPatients.png) |
| Find Patient | ![](Image/Patient/frmFindPatient.png) |

---

# 👨‍⚕️ Doctor Management

Manage doctors, specialties, consultation fees, and working schedules.

| Screen | Preview |
|---------|---------|
| Doctor List | ![](Image/Doctor/frmlistDoctor.png) |
| Add / Update Doctor | ![](Image/Doctor/frmAddUpdateDoctor.png) |
| Doctor Details | ![](Image/Doctor/frmshowDoctor.png) |
| Find Doctor | ![](Image/Doctor/frmFindDoctor.png) |

---

# 👤 User Management

System users, authentication and password management.

| Screen | Preview |
|---------|---------|
| Users List | ![](Image/User/frmlistUser.png) |
| Add / Update User | ![](Image/User/frmAddUpdateUser.png) |
| Change Password | ![](Image/User/frmChengePassword.png) |

---

# 🏥 Medical Service Types

Manage clinic services.

| Screen | Preview |
|---------|---------|
| Service Types | ![](Image/Medical%20Services/MangeServeces/ServecesType/frmMangedServeces.png) |
| Add / Update Service | ![](Image/Medical%20Services/MangeServeces/ServecesType/frmAddUpdateServeces.png) |

---

# 📅 Appointment Types

Configure appointment categories.

| Screen | Preview |
|---------|---------|
| Appointment Types | ![](Image/Medical%20Services/Appointment/AppointmentsType/frmMangedAppoinmetnType.png) |
| Add Appointment Type | ![](Image/Medical%20Services/Appointment/AppointmentsType/frmAddUpdateAppointmentType.png) |

---

# 🛎 Reception Station

Reception handles appointments and queue management.

| Screen | Preview |
|---------|---------|
| Main Reception | ![](Image/Medical%20Services/Appointment/Resaption/frmMianResapations.png) |
| Appointment List | ![](Image/Medical%20Services/Appointment/Resaption/frmListAppointment.png) |
| Add Appointment | ![](Image/Medical%20Services/Appointment/Resaption/frmAddUpdateAppoinmetn.png) |
| Queue | ![](Image/Medical%20Services/Appointment/Resaption/frmQueuList.png) |

---

# 👩‍⚕️ Nurse Station

The nurse records patient vital signs before the doctor examination.

| Screen | Preview |
|---------|---------|
| Nurse Dashboard | ![](Image/Medical%20Services/Visit/Nurse%20Station/frmMainNurse.png) |
| Patient Vital Signs | ![](Image/Medical%20Services/Visit/Nurse%20Station/frmnurse.png) |

---

# 👨‍⚕️ Doctor Station

Doctors perform diagnosis, create prescriptions, and manage visits.

| Screen | Preview |
|---------|---------|
| Doctor Dashboard | ![](Image/Medical%20Services/Visit/Doctor%20Station/frmMainDoctor.png) |
| Visits | ![](Image/Medical%20Services/Visit/Doctor%20Station/frmListVisit.png) |
| Add Visit | ![](Image/Medical%20Services/Visit/Doctor%20Station/frmAddVisit.png) |
| Update Visit | ![](Image/Medical%20Services/Visit/Doctor%20Station/frmUpdateVisit.png) |
| Visit Details | ![](Image/Medical%20Services/Visit/Doctor%20Station/frmshowVisit.png) |

---

# 💊 Pharmacy Station

Manage prescriptions, medicines and direct sales.

| Screen | Preview |
|---------|---------|
| Pharmacy Dashboard | ![](Image/Medical%20Services/Pharmycy/frmMainPharmicy.png) |
| Prescriptions | ![](Image/Medical%20Services/Pharmycy/frmAllPrescriptions.png) |
| Pending Prescriptions | ![](Image/Medical%20Services/Pharmycy/frmPrescriptionsActive.png) |
| Drug Management | ![](Image/Medical%20Services/Pharmycy/frmAddUpdateDrug.png) |
| Direct Sales | ![](Image/Medical%20Services/Pharmycy/frmDirectSales.png) |

---

# 💰 Cashier Station

Billing and payment management.

| Screen | Preview |
|---------|---------|
| Cashier Dashboard | ![](Image/Medical%20Services/Casher/frmMainCasher.png) |
| Bills | ![](Image/Medical%20Services/Casher/frmMangeBills.png) |
| Process Payment | ![](Image/Medical%20Services/Casher/frmProcessPayments.png) |
| Issue Invoice | ![](Image/Medical%20Services/Casher/frmIssueInvoice.png) |
| Medicine Return | ![](Image/Medical%20Services/Casher/frmReturnMedicine.png) |
| Edit Pending Prescription | ![](Image/Medical%20Services/Casher/frEditPindingPrescription.png) |

---

# 🏠 Main Dashboard

The central dashboard used by administrators to access every module.

![](Image/frmMain1.png)

---

# 🛠 Technologies Used

| Technology | Description |
|------------|-------------|
| Language | C# |
| Framework | .NET Framework 4.7.2 |
| UI | Windows Forms (WinForms) |
| Database | Microsoft SQL Server |
| Data Access | ADO.NET |
| Architecture | 3-Tier Architecture |
| Pattern | Repository Style + Business Objects |
| IDE | Visual Studio 2022 |
| Source Control | Git & GitHub |

---

# ⚙ Technical Highlights

The project implements several software engineering concepts:

- 3-Tier Architecture
- Object-Oriented Programming (OOP)
- Encapsulation
- Inheritance
- Polymorphism
- Separation of Concerns
- Reusable User Controls
- SQL Transactions
- SQL Bulk Copy
- Stored Procedures
- Validation Layer
- Generic Helper Classes
- Logging System
- Role-Based Navigation

---

# 🗄 Database Features

The SQL Server database includes:

- Doctors
- Patients
- People
- Users
- User Roles
- Doctor Specializations
- Working Days
- Appointments
- Appointment Types
- Visits
- Diagnoses
- Prescriptions
- Prescription Items
- Medicines
- Bills
- Payments
- Medical Services

The database was designed using relational modeling with foreign keys and transactional integrity.

---

# 🔒 Security Features

The system provides multiple security mechanisms:

- Login Authentication
- Password Management
- User Permissions
- Role-Based Access
- Input Validation
- SQL Transactions
- Exception Logging
- Duplicate Prevention
- Data Integrity Validation

---

# 🚀 Installation

## 1. Clone Repository

```bash
git clone https://github.com/yourusername/Clinic-Management-System.git
```

---

## 2. Open Solution

Open the solution using **Visual Studio 2022**.

---

## 3. Restore Database

- Open SQL Server Management Studio.
- Restore the provided ClinicDB database.
- Or execute the SQL scripts included with the project.

---

## 4. Configure Connection String

Update the connection string inside:

```
App.config
```

Example:

```xml
<connectionStrings>
    <add name="ClinicDB"
         connectionString="Server=YOUR_SERVER;
         Database=ClinicDB;
         Trusted_Connection=True;" />
</connectionStrings>
```

---

## 5. Build Project

```
Build
↓
Rebuild Solution
```

---

## 6. Run

Press

```
F5
```

or

```
Start Debugging
```

---

# 📦 Project Modules

✔ Login

✔ People Management

✔ Patient Management

✔ Doctor Management

✔ User Management

✔ Appointment Management

✔ Appointment Types

✔ Medical Services

✔ Reception Station

✔ Queue Management

✔ Nurse Station

✔ Doctor Station

✔ Pharmacy Station

✔ Cashier Station

✔ Bills

✔ Payments

✔ Prescriptions

✔ Medicines

✔ Visits

✔ Reporting Ready

---

# 📦 NuGet Packages

This project mainly relies on the standard .NET Framework libraries and does not require external NuGet packages.

Main libraries used include:

- System.Data.SqlClient
- System.Configuration
- System.Drawing
- System.Windows.Forms
- System.Xml
- Microsoft.CSharp

---

# 📈 Future Improvements

Potential future enhancements include:

- Dashboard Charts
- Statistical Reports
- Laboratory Module
- Radiology Module
- Inventory Management
- Multi-Branch Support
- SMS Notifications
- Email Notifications
- Barcode Printing
- QR Code Support
- REST API
- ASP.NET Web Version
- Mobile Application
- Cloud Synchronization

---

# 🤝 Contributing

Contributions are welcome.

If you have ideas, bug fixes, or improvements, feel free to fork the repository and submit a Pull Request.

---

# 👨‍💻 Author

**Albara Fahad Saleh Al-Huraisi**

Software Developer

GitHub:
> https://github.com/yourusername

LinkedIn:
> https://linkedin.com/in/yourprofile

---

# 📄 License

This project is intended for educational and portfolio purposes.

---

# ⭐ If you like this project

Please consider giving it a ⭐ on GitHub.

It really helps and motivates further development.

---

## Thank You ❤️
