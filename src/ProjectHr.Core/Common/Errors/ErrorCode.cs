using System.Collections.Generic;

namespace ProjectHr.Common.Errors;

public enum ErrorCode
{
    // Common


// Group - 1
    GroupSameName = 1000,
    GroupCannotFound = 1004,
    GroupReachedMaxStudentCount = 1005,
    GroupAlreadyAddedStudent = 1006,
    GroupHasActivityNotCompleted = 1007,

// Unique - 2
    WorkPhoneUnique = 2004,
    PersonalPhoneUnique = 2005,
    EmergencyPhoneUnique = 2006,
    WorkEmailAdressUnique = 2007,
    IdentityNumberUnique = 2008,
    StudentCannotBeParentOfHimselfEmail = 2009,
    StudentActivityTimeConfilict = 2010,
    StudentUpdateError = 2011,

// PriceGroup - 3
    PriceGroupSameName = 3000,
    PriceGroupSameUserCount = 3001,
    PriceGroupCannotFound = 3004,

// User - 4
    PhoneNumberAlreadyExist = 4000,
    EmailAlreadyExist = 4001,
    NameLength = 4002,
    SurnameLength = 4003,
    UserCannotFound = 4004,
    Invalid2FACode = 4005,
    PanelAccessWithoutEmail = 4006,
    PanelAccessDenied = 4007,
    FirstLoginChangePassword = 4008,
    ChangePasswordException = 4010,
    AlreadyEnabled2fa = 4009,

// Language - 5
    LanguageSameName = 5001,
    LanguageSameIso = 5002,
    LanguageNameRequired = 5003,
    LanguageCannotFound = 5004,
    LanguageNameLength = 5005,

    // Trainer  -6 
    TrainerCannotFound = 6004,
    TrainerActivityTimeConfilict = 6005,

    // Activitiy -7 
    ActivityNotFound = 7004,
    ActivityHasActivityInTheFuture = 7005,
    ActivityDayCannotFound = 7006,
    ActivityDayAlreadyConfirmed = 7007,
    ActivityDayAlreadyEncashed = 7008,
    ActivityThereIsNotEncashedStudent = 7009,
    ActivityUnAuthorizedRequest = 7010,
    ActivityConfirmedCannotUpdate = 7011,
    ActivityCannotUpdate = 7012,

    // Salary   -8 
    SalaryCannotFound = 8004,
    SalaryAlreadyCreated = 8005,
    SalaryConfirmedCannotBeUpdate = 8006,
    SalaryExistNotConfirmedActivity = 8007
}

public static class CustomError
{
    private static readonly Dictionary<ErrorCode, string> _errors = new()
    {
        { ErrorCode.ActivityCannotUpdate, "Aktivite guncellenemez." },
        {
            ErrorCode.SalaryExistNotConfirmedActivity,
            "Onaylanmayan aktite mevcuttur. Gunun onaylanmasi icin tum aktivitelerin onayinin olmasi gerekmektedir."
        },
        { ErrorCode.TrainerActivityTimeConfilict, "Egitimci icin ayni saatte aktivite mevcuttur." },
        { ErrorCode.StudentActivityTimeConfilict, "Ogrenci icin ayni saatte aktivite mevcuttur." },
        { ErrorCode.SalaryConfirmedCannotBeUpdate, "Onaylanmis canak bilgisi guncellenemez." },
        {
            ErrorCode.AlreadyEnabled2fa,
            "2 FA daha onceden acilistir."
        },
        {
            ErrorCode.StudentCannotBeParentOfHimselfEmail,
            "Panel erisiminin aktif edilmesi icin email bilginizin dolu olmasi gerekmektedir."
        },
        { ErrorCode.ActivityConfirmedCannotUpdate, "Aktivite onaylandıgı için değişiklik yapılamaz." },
        {
            ErrorCode.ActivityThereIsNotEncashedStudent,
            "Bu işlem admin/antrenör tarafından tahsil edilmediği için doğrulama sağayamamaktasınız.Lütfen tahsil işlemi tamamlandıktan sonra tekarar deneyiniz."
        },
        { ErrorCode.ActivityDayAlreadyEncashed, "Aktivite gunu icin daha onceden odeme alinmistir." },
        {
            ErrorCode.ActivityDayAlreadyConfirmed,
            "Aktivite gunu zaten onaylanmistir."
        },
        {
            ErrorCode.ActivityHasActivityInTheFuture,
            "Ileri tarihlerde aktivitesi oldugundan dolayi degisiklik yapilamaz."
        },
        {
            ErrorCode.ActivityDayCannotFound,
            "Aktivite icinde gonderilen tarihe ait kayit bulunamadi."
        },
        {
            ErrorCode.SalaryAlreadyCreated,
            "Daha onceden olusturulmus"
        },
        {
            ErrorCode.GroupHasActivityNotCompleted,
            "Grubun ileri tarihte etkinliği bulunmaktadır , bundan dolayı işleminize devam edilememektedir.Lütfen ilgili etkinlik tamamlandıktan sonra tekrar deneyiniz."
        },
        {
            ErrorCode.FirstLoginChangePassword,
            "Ilk giriste parola degistirilmesi zorunludur. Parola degistirilmesi icin gereken email gonderilmistir."
        },
        {
            ErrorCode.SalaryCannotFound,
            "Çanak kaydı bulunamadı"
        },
        {
            ErrorCode.TrainerCannotFound,
            "Eğitimci bulunamadı"
        },
        {
            ErrorCode.ActivityNotFound,
            "Aktivite bulunamadı"
        },
        {
            ErrorCode.GroupSameName,
            "Kaydetmeye çalıştığınız {0} sistemimizde zaten mevcuttur.Lütfen farklı bir isimde grup oluşturmayı deneyiniz ya da mevcut grup üzerinde düzenleme sağlayınız."
        },
        {
            ErrorCode.GroupCannotFound,
            "Grup bulunamadı."
        },
        {
            ErrorCode.WorkPhoneUnique,
            "İş telefon numarası başka bir kullanıcıyla aynı olamaz."
        },
        {
            ErrorCode.PersonalPhoneUnique,
            "Kişisiel Telefon numarası başka bir kullanıcıyla aynı olamaz."
        },
        {
            ErrorCode.EmergencyPhoneUnique,
            "Acil Telefon numarası başka bir kullanıcıyla aynı olamaz."
        },
        {
            ErrorCode.WorkEmailAdressUnique,
            "İş email adresi başka bir kullanıcıyla aynı olamaz. "
        },
        {
            ErrorCode.IdentityNumberUnique,
            "Kimlik Numarası başka bir kullanıcıyla aynı olamaz."
        },
        {
            ErrorCode.PriceGroupCannotFound,
            "Fiyat grubu bulunamadi."
        },
        {
            ErrorCode.PriceGroupSameName,
            "Kaydetmeye çalıştığınız {0} sistemimizde zaten mevcuttur.Lütfen farklı bir isimde grup fiyatı oluşturmayı deneyiniz ya da mevcut grup fiyatı üzerinde düzenleme sağlayınız."
        },
        {
            ErrorCode.PriceGroupSameUserCount,
            "Aynı kişi sayısına sahip grup zaten mevcuttur."
        },
        {
            ErrorCode.GroupReachedMaxStudentCount,
            "Gruptaki kişi sayısı max {0} adettir."
        },
        {
            ErrorCode.GroupAlreadyAddedStudent,
            "Öğrenci daha önce gruba eklenmiştir."
        },
        {
            ErrorCode.PhoneNumberAlreadyExist,
            "Bu telefon numarası başka bir kullanıcıya ait."
        },
        {
            ErrorCode.EmailAlreadyExist,
            "Bu email adresi başka bir kullanıcya ait."
        },
        {
            ErrorCode.NameLength,
            "Kullanıcının ismi 3-100 karakter uzunluğunda olmalıdır."
        },
        {
            ErrorCode.SurnameLength,
            "Kullanıcının soyadı 3-100 karakter uzunluğunda olmalıdır."
        },
        {
            ErrorCode.UserCannotFound,
            "Kullanıcı bulunamadı."
        },
        {
            ErrorCode.LanguageCannotFound,
            "Dil bulunamadı."
        },
        {
            ErrorCode.LanguageSameName,
            "Bu dil zaten kayıtlı."
        },
        {
            ErrorCode.LanguageSameIso,
            "Bu ISO kodu zaten kayıtlı."
        },
        {
            ErrorCode.LanguageNameRequired,
            "Dil adı boş olamaz."
        },
        {
            ErrorCode.LanguageNameLength,
            "Dil adı 3-32 karakter uzunluğunda olmalıdır."
        },
        {
            ErrorCode.Invalid2FACode,
            "Geçersiz 2 faktörlü kimlik doğrulama kodu"
        },
        {
            ErrorCode.PanelAccessWithoutEmail,
            "Bağlantılı öğrencinin panele erişebilmesi için mail adresini girmeniz gerekmektedir. Mail adresi girmek istemiyorsanız lütfen panel erişimi durumunu pasif hale getiriniz."
        },
        {
            ErrorCode.PanelAccessDenied,
            "Panele giriş yetkiniz şu an için bulunmamaktadır. Lütfen daha sonra tekrar deneyiniz ya da ilgili birimler ile iletişime geçiniz."
        }
    };

    public static Dictionary<ErrorCode, string> Errors => _errors;

    public static string GetError(ErrorCode code)
    {
        return _errors[code];
    }
}