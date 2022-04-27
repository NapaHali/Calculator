.PHONY: all pack clean run profile help

all: run

pack:
	powershell Compress-Archive ../../xkopac07_xvarga21_xbuday05_xmasle01 ../../xkopac07_xvarga21_xbuday05_xmasle01.zip

clean:
	# @ECHO OFF
	# SETLOCAL
	# SET "sourcedir=."
	# SET "keepfile=1.bat"
	# SET "keepdir=keep me"

	# FOR /d %%a IN ("%sourcedir%\*") DO IF /i NOT "%%~nxa"=="%keepdir%" RD /S /Q "%%a"
	# FOR %%a IN ("%sourcedir%\*") DO IF /i NOT "%%~nxa"=="%keepfile%" DEL "%%a"

	# GOTO :EOF

run:
	./bin/Release/Calculator.exe

profile:
	../stddev/bin/Release/stddev.exe < test.txt

help:
	@ECHO OFF
	echo "Pred spustenim programu je potrebne vykonat nasledovne:"
	echo "Vo Visual Studiu 2022 zkompilovat projekt Calculator a projekt stddev"
	echo "Precitat si uzivatelsku prirucku"
	echo "Precitat si licenciu prilozenu s danym produktom"
	echo "Po precitani potrebnych dokumentov mozeme spustit program Calculator.exe, ktory najdeme po zkompilovani v bin/release/Calculator.exe"
	@ECHO ON
	