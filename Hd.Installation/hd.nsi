
; version used later (e.g. for creating output file name)
!ifndef VERSION
	!define VERSION '2.0.0.0'
!endif

!ifndef CONFIGURATION
  !define CONFIGURATION 'Release'
!endif

!ifndef RELEASE_PATH
  !define RELEASE_PATH '${CONFIGURATION}'
!endif

;definition of output file
!ifdef OUTFILE
  OutFile "${OUTFILE}"
!else
  OutFile HelpDesk-${VERSION}-setup.exe
!endif

!ifndef ERR_FILE
  !define ERR_FILE 'ErrLog.ini'
!endif

;product name
!define PRODUCT_NAME 'TargetProcess Help Desk'

; Vars used latter in code
Var VDIRNAME
Var SRV_NAME
Var IIS_SRV_NAME
Var NEW_INSTALLATION
;only for update
Var TP_PATH
Var HD_TITLE
Var TP_LOGIN
Var TP_PWD
Var WIN_AUTH
Var HD_SCOPE
Var IS_PUBLIC
Var EXCEPTION

; application name
Name "TargetProcess Help Desk Portal"

; use modern user interface
!include "MUI.nsh"
; XML plugin need to write to web.config
!include "XML.nsh"
!include WordFunc.nsh
!insertmacro VersionCompare
!include LogicLib.nsh
!include "FileFunc.nsh"
!insertmacro DirState
!insertmacro un.DirState
!include "StrFunc.nsh"
${StrCase}

!macro ErrorHanlde gotoNoError
	ReadINIStr $0 "$PLUGINSDIR\tpe.ini" "Error" "Message"
	StrCmp $0 "" ${gotoNoError}
	MessageBox MB_OK|MB_ICONSTOP $0
!macroend

!macro ErrorHanldeAndContinue
	ReadINIStr $0 "$PLUGINSDIR\tpe.ini" "Error" "Message"
	${If} $0 != ""
		MessageBox MB_OK|MB_ICONSTOP $0
	${EndIf}
!macroend

!macro GetAttributeValueForNode Path Attrib AttribValue
	${xml::GotoPath} ${Path} $0
	${For} $R1 1 20
		${xml::GetAttribute} ${Attrib} $0 $1 
		${If} $0 == ${AttribValue}
			${xml::GetAttribute} "value" $0 $1
			${xml::NodeHandle} $1
			${break}
		${EndIf}
		${xml::NextSiblingElement} "add" $2 $1 ;move to next 'add' line
		push -1
		pop $1
	${Next}
!macroend

; if something wrong with installation process than user can abort it
!define MUI_ABORTWARNING

; use TP icons instaed of standard
!define MUI_ICON  icon32.ico
!define MUI_UNICON  icon32.ico

; it is need to reserve files which used before instllation was started 
ReserveFile "${NSISDIR}\Plugins\InstallOptions.dll"
ReserveFile "${NSISDIR}\Plugins\AccessControl.dll"
;ReserveFile "tp2.ini"
;ReserveFile "tp3.ini"
;ReserveFile "tp4.ini"

ReserveFile "Tools\InstallHelper.exe"

; we need custom pictures during installation process
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP orange.bmp
!define MUI_HEADERIMAGE_UNBITMAP orange.bmp
!define MUI_WELCOMEFINISHPAGE_BITMAP orange2.bmp
!define MUI_UNWELCOMEFINISHPAGE_BITMAP orange2.bmp

; this method is called first before installation begin
; loaded additional info for Custom pages
Function .onInit
	Call CheckDotNETVersion
	InitPluginsDir

	File "/oname=$PLUGINSDIR\InstallHelper.exe" "Tools\InstallHelper.exe"

	nsExec::Exec /TIMEOUT=20000 '$PLUGINSDIR\InstallHelper.exe -evhd "/TpIni:$PLUGINSDIR\tp.ini" "/ErrFile:$PLUGINSDIR\${ERR_FILE}"' 
	
	ReadINIStr $EXCEPTION "$PLUGINSDIR\${ERR_FILE}" "Error" "Message"
	${If} $EXCEPTION != ""
		; Do not log.
		MessageBox MB_OK|MB_ICONSTOP "$EXCEPTION"
		Abort
	${EndIf}

	!insertmacro MUI_INSTALLOPTIONS_EXTRACT "tp2.ini"
	!insertmacro MUI_INSTALLOPTIONS_EXTRACT "tp3.ini"
	!insertmacro MUI_INSTALLOPTIONS_EXTRACT "tp4.ini"
FunctionEnd

Page custom EnterDetectPrevPage LeaveDetectPrevPage

; custom page for creating virtual folder
Page custom EnterVFolderPage
Page custom EnterHDPage LeaveHDPage
Page custom EnterScopePage LeaveScopePage

; install files
Page instfiles InstFilesIn InstFilesOut
; installation completed
!define MUI_FINISHPAGE_SHOWREADME
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Start HelpDesk Portal after finishing installation." 
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION AutoStartUp
!insertmacro MUI_PAGE_FINISH

; scenario for de-installation
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
UninstPage instfiles
!insertmacro MUI_UNPAGE_FINISH

; installation langugage
!insertmacro MUI_LANGUAGE "English"



; Installation section called with 'Page instfiles'
Section "Install"
	SetOutPath $INSTDIR
	
	call ExtractFiles
	call CreateVirtualFolder
	call CreateProgramFilesFolder
	call UpdateWebConfig
	call UpdateRegistry
	call WriteUninstall

	CreateDirectory "$INSTDIR\wwwroot\Logs"
	AccessControl::GrantOnFile "$INSTDIR\wwwroot\Logs" "ASPNET" "GenericRead + GenericWrite + Delete"
	AccessControl::GrantOnFile "$INSTDIR\wwwroot\Logs" "IIS_WPG" "GenericRead + GenericWrite + Delete"

SectionEnd

; uninstall section
Section "Uninstall"
	; remove records in registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\HelpDesk"
	; restore $VDIRNAME
	ReadRegStr $VDIRNAME HKLM Software\TargetProcess\HelpDesk "vfolder"
	ReadRegStr $IIS_SRV_NAME HKLM Software\TargetProcess\HelpDesk "iissrv" 
	DeleteRegKey HKLM "Software\TargetProcess\HelpDesk"	
	; remove $VDIRNAME (virtual directory)
	nsExec::Exec /TIMEOUT=20000 '$INSTDIR\Tools\InstallHelper.exe -d "/VDirName:$VDIRNAME" "/IisSrvName:$IIS_SRV_NAME"'
	; remove Start->Programs->TP
	RMDir /r "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}"
	
	Var /GLOBAL dirState
	${un.DirState} "$SMPROGRAMS\TargetProcess" $dirState
	${If} $dirState == 0
		RMDir /r "$SMPROGRAMS\TargetProcess"
	${EndIf}
	; remove TP folder with all files
	RMDir /r "$INSTDIR"
SectionEnd


Function AutoStartUp
	; After installation complete open TargetProcess home page
	delete "$PLUGINSDIR\inst.ini"
	
	ReadRegStr $VDIRNAME HKLM Software\TargetProcess\HelpDesk "vfolder" 
	ReadRegStr $IIS_SRV_NAME HKLM Software\TargetProcess\HelpDesk "iissrv" 
	
	nsExec::Exec /TIMEOUT=20000 '$PLUGINSDIR\InstallHelper.exe -r "/InstIni:$PLUGINSDIR\inst.ini" "/VDirName:$VDIRNAME" "/IisSrvName:$IIS_SRV_NAME"'
	ReadINIStr $0 "$PLUGINSDIR\inst.ini" "Settings" "url"
	ExecShell "open" $0
FunctionEnd


Function InstFilesIn
	!insertmacro MUI_HEADER_TEXT "Installation" "Installation is in progress. It may take several minutes."
FunctionEnd

Function InstFilesOut
FunctionEnd


Function EnterDetectPrevPage
	StrCpy $NEW_INSTALLATION "yes"
	ReadRegStr $0 HKLM Software\TargetProcess\HelpDesk "version"
	${If} $0 == ""
		Abort
	${EndIf}
	call InitFromPreviousVersion
	!insertmacro MUI_HEADER_TEXT "Update" "You are updating your copy of Helpdesk Portal with Helpdesk Portal v.${VERSION}"
	!insertmacro MUI_INSTALLOPTIONS_DISPLAY "tp3.ini"
FunctionEnd

Function LeaveDetectPrevPage
	ReadINIStr $0 "$PLUGINSDIR\tp3.ini" "Field 1" "State"
	${If} $0 == 1
		Delete "$INSTDIR\wwwroot\bin\*.dll"
		Delete "$INSTDIR\wwwroot\bin\*.compiled"
		StrCpy $NEW_INSTALLATION "no"
	${EndIf}
FunctionEnd


; Show Database Dialog
Function EnterHDPage
	${If} $NEW_INSTALLATION == "no"
		Abort
	${EndIf}
	
	ReadINIStr $VDIRNAME "$PLUGINSDIR\tp.ini" "Field 6" "State"
	ReadINIStr $IIS_SRV_NAME "$PLUGINSDIR\tp.ini" "Field 5" "State"
	
	ReadRegStr $0 HKLM Software\TargetProcess\TargetProcess2 "vfolder" 
	ReadRegStr $1 HKLM Software\TargetProcess\TargetProcess2 "iissrv"

	Delete "$PLUGINSDIR\inst.ini"
	nsExec::Exec /TIMEOUT=20000 '$PLUGINSDIR\InstallHelper.exe -r "/InstIni:$PLUGINSDIR\inst.ini" "/VDirName:$0" "/IisSrvName:$1"'
	ReadINIStr $0 "$PLUGINSDIR\inst.ini" "Settings" "url"
	${If} $0 != ""
		WriteINIStr "$PLUGINSDIR\tp2.ini" "Field 5" "State" $0
	${EndIf}

	!insertmacro MUI_HEADER_TEXT "Help Desk Portal Configuration" "Provided values will be set in Web.config:"
	InstallOptions::dialog /NOUNLOAD "$PLUGINSDIR\tp2.ini"
	
	Pop $0
	InstallOptions::show
	Pop $0
FunctionEnd

Function LeaveHDPage
	ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Settings" "State"
	${If} $0 == 9
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 9" "State"
		IntOp $0 $0 ^ 1
		ReadINIStr $1 "$PLUGINSDIR\tp2.ini" "Field 8" "HWND"
		EnableWindow $1 $0
		ReadINIStr $1 "$PLUGINSDIR\tp2.ini" "Field 7" "HWND"
		EnableWindow $1 $0
		Abort
	${ElseIf} $0 == 0
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 5" "State"
		StrCpy $TP_PATH $0
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 6" "State"
		StrCpy $HD_TITLE $0
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 7" "State"
		StrCpy $TP_LOGIN $0
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 8" "State"
		StrCpy $TP_PWD $0
		ReadINIStr $0 "$PLUGINSDIR\tp2.ini" "Field 9" "State"
		StrCpy $WIN_AUTH $0
	${Else}
		Abort
	${EndIf}
FunctionEnd

Function EnterScopePage 
	${If} $NEW_INSTALLATION == "no"
	${AndIf} $HD_SCOPE != "none"
    ${AndIf} $IS_PUBLIC != "none"  
		Abort
	${EndIf}
    
    ${If} $HD_SCOPE == "Private"
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 1" "State" 1
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 2" "State" 0
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 3" "State" 0
    ${ElseIf} $HD_SCOPE == "Global"
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 1" "State" 0
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 2" "State" 1
        WriteINIStr "$PLUGINSDIR\tp4.ini" "Field 3" "State" 0                  
    ${EndIf}    
    
    
	!insertmacro MUI_HEADER_TEXT "Scope" "Please select scope for which Help Desk will be applied."
	!insertmacro MUI_INSTALLOPTIONS_DISPLAY "tp4.ini"
FunctionEnd

Function LeaveScopePage
	ReadINIStr $0 "$PLUGINSDIR\tp4.ini" "Field 1" "State"
    StrCpy $IS_PUBLIC "false"
    
	${If} $0 == 1      /* Private */
		StrCpy $HD_SCOPE "Private"
	${Else}
        ReadINIStr $0 "$PLUGINSDIR\tp4.ini" "Field 2" "State"
        ${If} $0 == 1  /* Global */
		StrCpy $HD_SCOPE "Global" 
    ${Else}            /* Public Global */
        StrCpy $IS_PUBLIC "true"
        StrCpy $HD_SCOPE "Global"
	${EndIf}
	${EndIf}
FunctionEnd

; Show Virtual Folder dialog
Function EnterVFolderPage
	${If} $NEW_INSTALLATION == "no"
		Abort
	${EndIf}
	!insertmacro MUI_HEADER_TEXT "Virtual Folder Parameters" "Specify location, IIS Web Site, and Virtual Folder name"

Retry:
	InstallOptions::dialog "$PLUGINSDIR\tp.ini"
	Pop $R0
	${If} $R0 != "back"
	${AndIf} $R0 != "Cancel"
		ReadINIStr $0 "$PLUGINSDIR\tp.ini" "Field 2" "State"
		StrCpy $VDIRNAME $0
		ReadINIStr $INSTDIR "$PLUGINSDIR\tp.ini" "Field 3" "State"
		ReadINIStr $IIS_SRV_NAME "$PLUGINSDIR\tp.ini" "Field 5" "State"
		${If} ${FileExists} $INSTDIR\*.*
			FindFirst $R1 $R2 "$INSTDIR\*.*"
			FindNext $R1 $R2
			FindNext $R1 $R2
			FindClose $R1
			${If} $R2 != ""
				MessageBox MB_OK "Directory $INSTDIR is not empty. Please specify another directory"
				Goto Retry
			${EndIf}
		${EndIf}
	${EndIf}
FunctionEnd

Function ExtractFiles
	File /r "${RELEASE_PATH}\*.*"
FunctionEnd

Function CreateVirtualFolder
/* if $VDIRNAME == "" updateWebSite else createVdir */

        StrCmp $VDIRNAME '' updateWebSite createVdir

        createVdir:
                   nsExec::Exec /TIMEOUT=20000 '$PLUGINSDIR\InstallHelper.exe -iis /add "/IisSrvName:$IIS_SRV_NAME" "/VDirName:$VDIRNAME" "/WwwRootPath:$INSTDIR\wwwroot"'
                   Return
        updateWebSite:
                   nsExec::Exec /TIMEOUT=20000 '$PLUGINSDIR\InstallHelper.exe -iis /update "/IisSrvName:$IIS_SRV_NAME" "/WwwRootPath:$INSTDIR\wwwroot"'
FunctionEnd

; Creates Start->Program->TP and places some URLs there.
Function CreateProgramFilesFolder
	CreateDirectory "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}"
	;WriteINIStr "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\Open HelpDesk.url" "InternetShortcut" "URL" "http://localhost/$VDIRNAME"
	WriteINIStr "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\TargetProcess Home.url" "InternetShortcut" "URL" "http://www.targetprocess.com"
	;WriteINIStr "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\Support Forum.url" "InternetShortcut" "URL" "http://support.targetprocess.com/Default.aspx?g=forum"
	;CreateShortCut "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\User Guide.lnk" "$INSTDIR\TargetProcess v2 User Guide.pdf"
	CreateShortCut "$SMPROGRAMS\TargetProcess\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	;WriteINIStr "$SMPROGRAMS\${PRODUCT_NAME}\Shortcut.url" "InternetShortcut" "IconFile" "$INSTDIR\file.dll"
	;WriteINIStr "$SMPROGRAMS\${PRODUCT_NAME}\Shortcut.url" "InternetShortcut" "IconIndex" "0"		
FunctionEnd

Function UpdateWebConfig
	
	DetailPrint "Updating Web.config and Database..."

	${xml::LoadFile} "$INSTDIR\wwwroot\web.config" $0

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "TargetProcessPath"
	${xml::SetAttribute} "value" "$TP_PATH" $0

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "AdminLogin"
	${xml::SetAttribute} "value" "$TP_LOGIN" $0

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "AdminPassword"
	${xml::SetAttribute} "value" "$TP_PWD" $0
	
	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "Title"
	${xml::SetAttribute} "value" "$HD_TITLE" $0
	
	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "ActiveDirectoryMode"
	${If} $WIN_AUTH == 1
		${xml::SetAttribute} "value" "true" $0
	${Else}
		${xml::SetAttribute} "value" "false" $0
	${EndIf}

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "Scope"
	${xml::SetAttribute} "value" "$HD_SCOPE" $0
    
    !insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "IsPublic"
    ${xml::SetAttribute} "value" "$IS_PUBLIC" $0
    
	${xml::SaveFile} "$INSTDIR\wwwroot\web.config" $0	

	${xml::Unload}

FunctionEnd

Function UpdateRegistry
	WriteRegStr HKLM Software\TargetProcess\HelpDesk "version" ${VERSION}
	WriteRegStr HKLM Software\TargetProcess\HelpDesk "path" $INSTDIR
	WriteRegStr HKLM Software\TargetProcess\HelpDesk "vfolder" $VDIRNAME
	WriteRegStr HKLM Software\TargetProcess\HelpDesk "iissrv" $IIS_SRV_NAME
FunctionEnd

Function WriteUninstall
	; write uninstall information (reqired to uninstall TP through Control Panel)
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\HelpDesk" "DisplayName" "${PRODUCT_NAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\HelpDesk" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\HelpDesk" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\HelpDesk" "NoRepair" 1
	WriteUninstaller "uninstall.exe"
FunctionEnd

Function CheckDotNETVersion
	Push $0
	Push $1

   
	Push $0
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
    ${If} $0 >= 1 
        GOTO dotNet4Found
    ${Else}
        GOTO dotNet4NotFound
    ${EndIf}    
    Pop $0 
    

    dotNet4NotFound:
		MessageBox MB_OK|MB_ICONSTOP ".NET v4.0 runtime library is not installed."
		Abort
    GOTO ExitLabel
        
    dotNet4Found:
        
    ExitLabel:
	
	Pop $1
	Pop $0
FunctionEnd

Function InitFromPreviousVersion
	ReadRegStr $INSTDIR HKLM Software\TargetProcess\HelpDesk "path" 
	ReadRegStr $VDIRNAME HKLM Software\TargetProcess\HelpDesk "vfolder" 
	ReadRegStr $IIS_SRV_NAME HKLM Software\TargetProcess\HelpDesk "iissrv" 

	; load web.config
	${xml::LoadFile} "$INSTDIR\wwwroot\web.config" $0
	${If} $0 != 0
		Abort
	${EndIf}
	
	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "TargetProcessPath"
	${xml::GetAttribute} "value" $TP_PATH $1 
	
	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "AdminLogin"
	${xml::GetAttribute} "value" $TP_LOGIN $1 

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "AdminPassword"
	${xml::GetAttribute} "value" $TP_PWD $1 

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "Title"
	${xml::GetAttribute} "value" $HD_TITLE $1 
    
    !insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "IsPublic"
    ${If} $1 == -1
        StrCpy $IS_PUBLIC "none"
    ${Else}
        ${xml::GetAttribute} "value" "$IS_PUBLIC" $1
    ${EndIf}
        
	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "Scope"
	${If} $1 == -1
		StrCpy $HD_SCOPE "none"
	${Else}
		${xml::GetAttribute} "value" "$HD_SCOPE" $1
        
        StrCmp $HD_SCOPE "Company" MigrateCompanyScope Continue 
        MigrateCompanyScope:
            StrCpy $HD_SCOPE "Global"
        Continue:                    
	${EndIf}

	!insertmacro GetAttributeValueForNode "/configuration/appSettings/add" "key" "ActiveDirectoryMode"
	${xml::GetAttribute} "value" $0 $1 
	
	${StrCase} $1 "$0" "L"

	${If} $0 == "true"
		Push 1
		Pop $WIN_AUTH
	${EndIf}
	${xml::Unload}

FunctionEnd



