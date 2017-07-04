﻿module Paket.ProjectFile.TargetFrameworkSpecs

open Paket
open NUnit.Framework
open FsUnit
open Paket.TestHelpers

let portable = TargetProfile.FindPortable [ DotNetFramework FrameworkVersion.V4_5; Windows WindowsVersion.V8; WindowsPhone WindowsPhoneVersion.V8; WindowsPhoneApp WindowsPhoneAppVersion.V8_1 ]
    
let TestData: obj[][] =
    [|
        // project file name, 
        //  expected TargetProfile 
        //  expected TargetProfile.ToString
        //  expected TargetFramework
        [|"Project2.fsprojtest";
            (SinglePlatform(DotNetFramework FrameworkVersion.V4));
            "net40";
            (DotNetFramework FrameworkVersion.V4)|];
        [|"Empty.fsprojtest";
            (SinglePlatform(DotNetFramework FrameworkVersion.V4));
            "net40";
            (DotNetFramework FrameworkVersion.V4)|];
        [|"NewSilverlightClassLibrary.csprojtest";
            (SinglePlatform(Silverlight SilverlightVersion.V5));
            "sl5";
            (Silverlight SilverlightVersion.V5)|];
        [|"FSharp.Core.Fluent-3.1.fsprojtest";
            portable;
            "portable-net45+win8+wp8+wpa81";
            (DotNetFramework FrameworkVersion.V4_5)|];
        [|"MicrosoftNetSdkWithTargetFramework.csprojtest";
            (SinglePlatform(DotNetStandard DotNetStandardVersion.V1_4));
            "netstandard1.4";
            (DotNetStandard DotNetStandardVersion.V1_4)|];
    |]
    
[<Test>]
let ``should detect profile259`` () =
    portable
    |> shouldEqual (PortableProfile PortableProfileType.Profile259)
    
[<Test>]
[<TestCaseSource("TestData")>]
let ``should detect the correct framework on test projects`` projectFile expectedProfile expectedProfileString expectedTargetFramework =
    ensureDir()
    let p = ProjectFile.TryLoad("./ProjectFile/TestData/" + projectFile).Value
    p.GetTargetProfile() |> shouldEqual expectedProfile
    p.GetTargetProfile().ToString() |> shouldEqual expectedProfileString

