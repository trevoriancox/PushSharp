using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;

//[assembly: Permission(Name="@PACKAGE_NAME@.permission.C2D_MESSAGE", ProtectionLevel="signature")]
[assembly: UsesPermission("@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission("com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission("android.permission.INTERNET")]
[assembly: UsesPermission("android.permission.WAKE_LOCK")]
[assembly: UsesPermission("android.permission.GET_ACCOUNTS")]