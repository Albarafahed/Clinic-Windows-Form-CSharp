# Clinic Management System 🏥

نظام إدارة عيادة طبية متكامل ومحكم مبني باستخدام لغة **C# WinForms** واعتماد معمارية الطبقات الثلاث (**3-Tier Architecture**). يهدف النظام إلى أتمتة العمليات الطبية، وإدارة شؤون المرضى، والأطباء، والمستخدمين بكفاءة عالية وأمان فائق.

---

## 🏗️ البنية المعمارية وهندسة النظام
تم بناء النظام بمعايير برمجية تضمن القابلية للتوسع (Scalability) وسهولة الصيانة:
* **Presentation Layer:** واجهات مستخدم احترافية وتفاعلية.
* **Business Logic Layer:** تطبيق قواعد العمل الصارمة (Business Rules) والتحقق التلقائي.
* **Data Access Layer:** تعامل آمن ومباشر مع SQL Server لضمان أداء عالٍ وحماية ضد ثغرات الاختراق.

---

## 📅 التحديثات والميزات (Module Showcase)

### 👥 إدارة الأشخاص (Person Management)
نظام مركزي لإدارة بيانات الأشخاص (مرضى، أطباء، مستخدمين).

| الشاشة | اللقطة |
| :--- | :--- |
| **قائمة الأشخاص** | ![List People](Image/Person/frmlistPeople.png) |
| **إضافة/تعديل شخص** | ![Add/Update Person](Image/Person/frmaddUpdateperson.png) |
| **تفاصيل الشخص** | ![Show Person](Image/Person/frmShowPerson.png) |

---

### 🩺 إدارة المرضى (Patient Management)
إدارة كاملة للملفات الطبية للمرضى مع حماية ضد التكرار وتحقق من صحة البيانات.

| الشاشة | اللقطة |
| :--- | :--- |
| **قائمة المرضى** | ![List Patients](Image/Patient/frmListPatients.png) |
| **إضافة/تعديل مريض** | ![Add/Update Patient](Image/Patient/frmAddUpdatePatient.png) |
| **تفاصيل المريض** | ![Show Patient](Image/Patient/frmShowPatients.png) |

---

### 🔑 إدارة المستخدمين (User Management)
تحكم كامل بصلاحيات الدخول وحماية الحسابات.

| الشاشة | اللقطة |
| :--- | :--- |
| **قائمة المستخدمين** | ![List Users](Image/User/frmlistUser.png) |
| **إضافة/تعديل مستخدم** | ![Add/Update User](Image/User/frmAddUpdateUser.png) |
| **تغيير كلمة المرور** | ![Change Password](Image/User/frmChengePassword.png) |

---

## 🖥️ الواجهة الرئيسية (Dashboard)
المركز العصبي للنظام الذي يربط كافة الأقسام.

![Main Dashboard](Image/frmMain.png)

---

## 🛠️ التقنيات المستخدمة
* **اللغة:** C# (.NET Framework)
* **قاعدة البيانات:** Microsoft SQL Server
* **نمط التصميم:** 3-Tier Architecture / Vertical Slicing
* **إدارة المستودع:** Git

---

## 🚀 كيفية التشغيل
1. قم بعمل `Clone` للمستودع:
   ```bash
   git clone [https://github.com/your-username/Clinic_System.git](https://github.com/your-username/Clinic_System.git)