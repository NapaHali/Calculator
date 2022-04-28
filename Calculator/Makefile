.PHONY: all pack clean run profile help

all: run

pack:
	powershell Compress-Archive ../../xkopac07_xvarga21_xbuday05_xmasle01 ../../xkopac07_xvarga21_xbuday05_xmasle01.zip

clean:
	# powershell Remove-Item ../1.bat -Exclude *.*

run:
	./bin/Release/Calculator.exe

profile:
	../stddev/bin/Release/stddev.exe < ../stddev/bin/Release/test.txt

help:
	@echo POSTUP URCENY PRE VYVOJARA
	@echo Pred spustenim programu je potrebne vykonat nasledovne:
	@echo 1. Vo Visual Studiu 2022 zkompilovat projekt Calculator a projekt stddev
	@echo 2. Precitat si uzivatelsku prirucku a programovu prirucku
	@echo 3. Precitat si licenciu prilozenu s danym produktom
	@echo 4. Po precitani potrebnych dokumentov mozeme spustit program Calculator.exe, ktory najdeme v bin/release/
	@echo 
	@echo POSTUP URCENY PRE UZIVATELA
	@echo Pred spustenim programu je potrebne vykonat nasledovne:
	@echo 1. Otvorit setup.exe, ktory je prilozeny v archive
	@echo 2. Pomocnik pri instalacii vam ukaze vsetky potrebne informacie, moznosti a licenciu, ktore si treba podrobne prestudovat. Pri poruseni zmluvnych podmienok hrozi trestnopravne stihanie!
	@echo 3. Po ukonceni instalacii a uspesnom nainstalovani produktu, mozeme program spustit dvojitym kliknutim na Calculator.exe
	@echo 4. Kalkulacka by sa mala otvorit a byt pripravena k pouzitiu
	