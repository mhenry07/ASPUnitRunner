@echo off
SET SRC="CommonAssemblyInfo.template.cs"
SET DEST="CommonAssemblyInfo.cs"
PUSHD "%~dp0"
IF NOT EXIST %DEST% COPY %SRC% %DEST%
POPD
