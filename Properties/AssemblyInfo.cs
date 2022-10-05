using System.Reflection;
using System.Runtime.InteropServices;

using MelonLoader;
using StateSaver;

[assembly:AssemblyTitle(StateSaver.BuildInfo.Author)]
[assembly:AssemblyDescription("")]
[assembly:AssemblyConfiguration("")]
[assembly:AssemblyCompany(StateSaver.BuildInfo.Company)]
[assembly:AssemblyProduct(StateSaver.BuildInfo.Name)]
[assembly:AssemblyCopyright("Created by " + StateSaver.BuildInfo.Author)]
[assembly:AssemblyTrademark(StateSaver.BuildInfo.Company)]
[assembly:AssemblyCulture("")]
[assembly:ComVisible(false)]
[assembly:Guid("fb4b24ab-ec76-404d-971b-19cf9f079115")]
[assembly:AssemblyVersion(StateSaver.BuildInfo.Version)]
[assembly:AssemblyFileVersion(StateSaver.BuildInfo.Version)]
[assembly:MelonInfo(typeof(StateSaver.Main), StateSaver.BuildInfo.Name,
                    StateSaver.BuildInfo.Version, StateSaver.BuildInfo.Author)]
// [assembly:MelonGame("Stress Level Zero", "BONELAB")]
