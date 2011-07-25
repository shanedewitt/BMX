BMXADOF	; IHS/CIHA/GIS - RPC CALL FOR EXTENDED FUNCTIONALITY OF BMXNet UTILITIES ;
	;;2.31;BMX;;Jul 25, 2011
	; THIS IS THE ADO RECORDSET FILER: ADO -> FILEMAN
	; VISIT FILE UPDATES REPRESENT A SPECIAL CASE HTAT IS MANAGED IN BMXADOF1
	; INCLUDES TRANSACTION CONTROLS
	; 
	; 
	;
	N DAS,FILE,DATA,OUT S DAS=7,FILE=19707.82,DATA="2.02|120/83" D FILE(.OUT,FILE,DAS,DATA) W !,OUT Q
	; 
FILED(OUT,FILE,DAS,DATA)	; RPC CALL: UNIVERSAL FILEMAN RECORD UPDATER UTILITY
	D DEBUG^%Serenji("FILE^BMXADOF(.OUT,FILE,DAS,DATA)")  ; DEBUGGER ENTRY POINT
	; K ^GREG S ^GREG("OUT")=$G(OUT),^("FILE")=$G(FILE),^("DAS")=$G(DAS),^("DATA")=$G(DATA) D FILE(.OUT,FILE,DAS,DATA)
	Q
	; 
FILEX(OUT,FILE,DAS,DATA)	; EP - RPC CALL: INSURES THAT BMXIEN IS VALID - MOJO ONLY
	I '$L($G(DATA)) D
	. S DATA="",%=""
	. F  S %=$O(DATA(%)) Q:'%  S DATA=DATA_DATA(%) ; CONVERT DATA ARRAY INTO A DATA STRING
	. Q
	I '$L(DATA) Q
	I DATA["999|" S DAS=+$P(DATA,"999|",2) I 'DAS S DAS="" ; FORCE NEW ENTRY
	D FILE(.OUT,FILE,$G(DAS),DATA)
	Q
	; 
FILE(OUT,FILE,DAS,DATA)	;EP - RPC CALL: UNIVERSAL FILEMAN RECORD UPDATER UTILITY
	; 
	; OUT = OUTBOUND MESSAGE RETURNED TO CALLINING APP.  'OK'=SUCCESSFUL TRANSACTION, 'OK|5' NEW RECORD DAS=5 ADDED
	;   IF TRANSACTION FAILS, AN ERROR MESSAGE IS PASSED
	; FILE = VALID FILEMAN FILE OR SUB-FILE NUMBER - WHERE UPDATE IS TO OCCUR
	; DAS = THE DA STRING - TYPICALLY THE FILE INTERNAL ENTRY NUMBER OF THE RECORD TO BE UPDATED
	;   IF THIS IS A SUB-FILE, DAS MUST BE PRECEDED BY PARENT DAS(S) IN COMMA SEPARATED STRING - TOP TO BOTTOM ORDER
	;   DAS MAY BE PRECEDED BY '+' = ALL FIELDS ARE MANDATORY (REQD FOR TRANSACTION) OR '-' = DELETE THIS ENTRY
	;   IF DAS STRING = NULL OR = '+', THIS MEANS ADD A NEW RECORD WITH DATA SUPPLIED IN DATA PARAMETER
	;   EXAMPLES OF DAS STRINGS: '1' (EDIT RECORD #1), '5,2,-7' (DELETE RECORD #7 IN 3RD LEVEL SUBFILE)
	; DATA = DATA STRING OR ARRAY REFERENCE.  DATA CAN BE PASSED USING THE .PARAM SYNTAX
	;   DATA STRING FORMAT: FIELD#|VALUE_$C(30)_FIELD#|VALUE_$C(30)_...FIELD#|VALUE_$C(30)
	;     $C(30) [AKA EOR] IS THE DATA ELEMENT SEPARATOR
	;     $C(30) IS USED AS THE DATA DELIMITER BECAUSE OTHER CHARACTERS LIKE '^' COULD APPEAR IN THE VALUE PIECE!
	;   EA FIELD# MAY BE PRECEED BY '+' = MANDATORY (REQD FOR TRANSACTION) OR '-' = DELETE THE VALUE OF THIS FIELD
	;      EXAMPLE: ".03|1/5/46"_EOR_"-.02|"_EOR_"+.09|139394444"_EOR  NOTE -.02| IS SAME AS .02|@ OR .02|
	;   '+' IN FRONT OF THE DAS IS THE SAME AS PUTTING A '+' IN FRONT OF EVERY FIELD# IN THE DATA STRING
	; 
	; 
	; 
	N VENDUZ,VUZ
	M VENDUZ=DUZ S VUZ=$C(68,85,90)
	N OREF,CREF,DIC,DIE,DA,DR,X,Y,%,I,FLD,CNT,FNO,VAL,@VUZ,TFLG,DFLG,TOT,UFLG,XTFLG,GTFLG,GDFLG,LVLS,IENS
	I $G(FILE)=9000010 N AUPNPAT,AUPNDOB,AUPNDOD,AUPNVSIT,AUPNTALK,APCDOVRR S (APCDOVRR,AUPNTALK)=1 ; THE VISIT FILE IS UPDATED IN THIS TRANSACTION
	X ("M "_$C(68,85,90)_"=VENDUZ S "_$C(68,85,90)_"(0)="_$C(34,64,34)) K VENDUZ ; ELININATES PERMISSION PROBLEMS
	S OUT="",FLD="",GTFLG=0,GDFLG=0
	S X="MERR^BMXADOF",@^%ZOSF("TRAP") ; SET MUMPS ERROR TRAP
	I '$D(^DD(+$G(FILE))) S OUT="Invalid file number" Q  ; FILE # MUST BE VALID
	S DAS=$G(DAS) I $E(DAS)="," S DAS=$E(DAS,2,99) ; ACCURATE IF NON SUB-FILE DAS STRING DOSN'T CONTAIN A ","
	S LVLS=$L(DAS,",")
	S %=FILE F CNT=1:1 S %=$G(^DD(%,0,"UP")) I '% Q  ; COUNT FILE/SUB-FILE LEVELS IN THE DATA DICTIONARY
	I LVLS'=CNT S OUT="Invalid DAS string" Q  ; LEVELS IN DAS STRING MUST MATCH LEVELS IN THE DATA DICTIONARY
	I $E(DAS)="-" S DAS=$E(DAS,2,99),GDFLG=1 ; GLOBAL DELETE FLAG
	I $E(DAS)="+" S DAS=$E(DAS,2,99),GTFLG=1 ; GLOBAL TRANSACTION FLAG, ROLLBACK IF ANY FIELD FAILS TO UPDATE
	I LVLS>1 F I=1:1:LVLS D  I DAS="ERR" S OUT="Invalid DAS string" Q  ; MAKE DAS ARRAY.  MIRRORS THE DA() ARRAY
	. I I=LVLS S DAS=$P(DAS,",",I) Q  ; SET DAS OF SUBFILE
	. S %=$P(DAS,",",I) I '% S DAS="ERR" Q
	. S DAS(LVLS-I)=% ; SET DAS(S) OF PARENT FILE(S). LIKE DA(), THE LARGER THE DAS SUBSCRIPT, THE HIGHER THE LEVEL
	. Q
	I DAS="ERR" S OUT="Update cancelled.  Invalid DAS string" Q
	I DAS="Add"!(DAS="ADD") S DAS=""
	S %=$E(DAS) I %="-" S GDFLG=1,DAS=$E(DAS,2,99) ; YET ANOTHER WAY TO SET GLOBAL DELETE FLAG
	S %=$$REF(FILE,.DAS) ; GET OPEN REF, CLOSED REF, AND IENS STRING
	S OREF=$P(%,"|"),CREF=$P(%,"|",2),IENS=$P(%,"|",3) I $L(OREF),$L(CREF)
	E  S OUT="Update cancelled.  Invalid file definition/global reference" Q  ; ERROR REPORT
	I DAS,'$D(@CREF@(DAS)) S OUT="Update cancelled. Invalid DAS" Q  ; IF THERE IS AN DAS, IT MUST BE VALID
	I '$G(DAS),FILE=9000010,'$$VVAR^BMXADOF2(DATA) Q  ; VISIT FILE ADD REQUIRES THAT SPECIAL VARIABLES BE PRESENT AND VALID
	I 'GDFLG,DAS,DATA[".01|@" S GDFLG=1 ; ALTERNATE WAY TO SET GLOBAL DELETE FLAG: REMOVE .01 FIELD
	I GDFLG,'DAS S OUT="Deletion cancelled. Missing DAS" Q  ; CAN'T DO DELETE WITHOUT AN DAS
	I GDFLG D DIK(OREF,DAS) S OUT="Record deleted|"_DAS Q  ; DELETE AND QUIT
	S UFLG=$S($G(DAS):"E",1:"A") ; SET UPDATE FLAG: ADD OR EDIT
	I '$L($G(DATA)) D  I '$L($G(DATA)) S OUT="Update cancelled.  Missing/invalid data string" Q  ; COMPRESS DATA ARRAY INTO A SINGLE STRING
	. S DATA="",%=""
	. F  S %=$O(DATA(%)) Q:'%  S DATA=DATA_DATA(%) ; CONVERT DATA ARRAY INTO A DATA STRING
	. Q
	S %=$L(DATA) S %=$E(DATA,%-1,%) D  ; CHECK FOR PROPER TERMINATION OF DATA STRING
	. I %=$C(30,31) Q  ; PROPER TERMINATION
	. I $E(%,2)=$C(30) S DATA=DATA_$C(31) Q
	. I $E(%,2)=$C(31) S DATA=$E(DATA,1,$L(DATA-1))_$C(30,31)
	. S DATA=DATA_$C(30,31)
	. Q
	S TOT=$L(DATA,$C(30)) I 'TOT S OUT="Update cancelled.  Missing data string" Q
SPEC	S DATA=$$SPEC^BMXADOFS(FILE,DATA,UFLG) ; BASED ON FILE IEN, SPECIAL MODS MAY BE MADE TO THE DATA STRING
	S TOT=$L(DATA,$C(30)) I 'TOT S OUT="Update cancelled.  SPEC analysis failed." Q
	F CNT=1:1:TOT S %=$P(DATA,$C(30),CNT) I $L(%) S DATA(CNT)=% ; BUILD PRIMARY FIELD ARRAY
	S %=$G(DATA(1)) I %=""!(%=$C(31)) S OUT="Update cancelled.  Missing data string" Q
	S %=DATA(CNT) I %[$C(31) S %=$P(%,$C(31),1),DATA(CNT)=% ; STRIP OFF END OF FILE MARKER
	F CNT=1:1:TOT S X=$G(DATA(CNT)) I $L(X) D  ; BUILD SECONDARY FIELD ARRAY
	. S TFLG=0,DFLG=0
	. I $E(X)="+" S TFLG=1,X=$E(X,2,999),$P(FLD,U)=1
	. I $E(X)="-" S DFLG=1,X=$E(X,2,999)
	. S FNO=$P(X,"|"),VAL=$P(X,"|",2)
	. I '$D(^DD(FILE,+$G(FNO),0)) S:$L(OUT) OUT=OUT_"~" S OUT=OUT_FNO_"|Invalid field number" Q
	. I DFLG,VAL'="" S:$L(OUT) OUT=OUT_"~" S OUT=OUT_FNO_"|Invalid deletion syntax" Q  ; CANT DELETE IF A VALUE IS SENT
	. I VAL="@" S DFLG=1 ; SYNC DFLG AND VAL
	. S FLD(FNO)=VAL_U_TFLG_U_DFLG
	. I FNO=.01,TFLG S $P(FLD,U,2)=1
	. Q
	I $P($G(FLD(.01)),U,3),UFLG="A" S OUT="Record deletion cancelled.  Missing DAS" Q  ; CAN'T DELETE A RECORD WITHOUT A VALID DAS
	I $P($G(FLD(.01)),U,3)!($G(GDFLG)) S UFLG="D" ; DELETION
DELREC	I UFLG="D" D DIK(OREF,DAS) S OUT="OK" Q  ; DELETE THE RECORD
	I UFLG="A",'$L($P($G(FLD(.01)),U)) S OUT="Record addition cancelled.  Missing .01 field" Q  ; CAN'T ADD A RECORD WITHOUT A VALID .01 FIELD
DINUM	I UFLG="A",$G(^DD(FILE,.01,0))["DINUM=X" D  ; IF DINUM'D RECORD EXISTS, SWITCH TO MOD MODE
	. S %=FLD(.01)
	. I $E(%)="`" S %=+$E(%,2,99)
	. I '$D(@CREF@(%,0)) Q  ; OK TO ADD BRAND NEW RECORD BUT EXISTING RECORDS MUST BE EDITED
	. K FLD(.01)
	. S DAS=%,UFLG="E"
	. Q
ADDREC	I UFLG="A" D ADD(OREF) Q  ; ADD A NEW ENTRY TO A FILE
EDITREC	I UFLG="E" D EDIT(OREF,DAS) Q  ; EDIT AN EXISTING RECORD
	Q
	;
DIK(DIK,DA)	; DELETE A RECORD
	; PATCHED BY GIS 9/28/04 TO FIX PROBLEMS WITH SUBFILE DELETION
	I '$G(DAS(1)) G DIK1 ; CHECK FOR SUBFILE DELETION
	N DA,IENS,I,DIK
	I '$G(FILE) Q
	S I=0,IENS=DAS_","
	M DA=DAS
	F  S I=$O(DAS(I)) Q:'I  S IENS=IENS_DAS(I)_","
	S DIK=$$ROOT^DILFD(FILE,IENS) I '$L(DIK) Q
DIK1	D ^DIK
	D ^XBFMK
	Q
	;
ADD(DIC)	; ADD A NEW ENTRY TO A FILE
	N X,Y,%,DA,DN,UP,SB,DNODE,ERR
	S X=$P($G(FLD(.01)),U) I '$L(X) S OUT="Unable to add a new record" Q
	S X=$$POINT(FILE,.01,X) ; ADD ACCENT GRAV IF NECESSARY
	S X=""""_X_"""" ; FORCE A NEW ENTRY
	S DIC(0)="L"
	I $O(DAS(0)) D  I $G(ERR) S Y=-1 G AFAIL ; GET DIC("P") IF NECESSARY
	. S %=0 F  S %=$O(DAS(%)) Q:'%  S DA(%)=DAS(%) ; CREATE THE DA ARRAY
	. S UP=$G(^DD(FILE,0,"UP")) I 'UP S ERR=1 Q
	. S SB=$O(^DD(UP,"SB",FILE,0)) I 'SB S ERR=1 Q
	. S DIC("P")=$P($G(^DD(UP,SB,0)),U,2) I '$L(DIC("P")) S ERR=1 Q
	. S DN=DIC_"1,0)" I $D(DN) Q
	. S @DN=(U_DIC("P")_U_U) ; CREATE THE DICTIONARY NODE
	. Q
ADIC	D ^DIC
AFAIL	I Y=-1 S OUT="Unable to add a new record" G AX
	I $O(FLD(0)) D EDIT(DIC,+Y) Q
	S OUT="OK"_"|"_+Y
AX	D ^XBFMK
	Q
	;
EDIT(DIE,DA)	; EDIT AN EXISTING RECORD
	N DR,RFLG,ERR,FNO,VAL,TFLG,RESULT,MSG,DIERR,DISYS,SF,APCDALVR
	S FNO=0,DR="",APCDALVR=""
	I UFLG="A" S OUT="OK New record added|"_DA
	F  S FNO=$O(FLD(FNO)) Q:'FNO  S X=FLD(FNO) I $L(X) D  I $G(RFLG) Q  ; CHECK EA FIELD AND BUILD THE DR STRING AND ERROR STRING
	. S VAL(FNO)=$P(X,U),TFLG=$P(X,U,2) I '$L(VAL(FNO)) Q
	. S SF=$$WP(FILE,FNO)
	. I SF D WORD(FILE,DA,FNO,CREF,VAL(FNO)) Q  ; WORD PROCESSING FIELDS MANAGED SEPARATELY
	. S VAL(FNO)=$$POINT(FILE,FNO,VAL(FNO)) ; ADD ACCENT GRAV IF NECESSARY
	. K ERR,RESULT
	. I VAL(FNO)="@"!(VAL(FNO)="") S RESULT="@"
	. I FNO=.01,UFLG="A" S:$E(VAL(.01))="`" VAL(.01)=$E(VAL(.01),2,999) Q  ; NO NEED TO EDIT THE .01 FIELD OF A RECORD THAT HAS JUST BEEN CREATED
	. I FILE\1=9000010,$L($P(FILE,".",2))=2,UFLG="E",(FNO=.02!(FNO=.03)) Q  ; CAN'T EDIT EXISTING PT AND VISIT FIELDS OF V FILES
	. I FILE\1=9000010,$L($P(FILE,".",2))=2,UFLG="A",FNO=.03,VAL(.03)?1"`"1.N S %=+$E(VAL(.03),2,99) I $D(^AUPNVSIT(%,0)) S RESULT=% G E1
	. I FILE=9000011,FNO=.07,VAL(.07)?1.N S RESULT=VAL(.07) G E1 ; THE VALIDITY CHECK FAILS - SO BYPASS THIS
CHK	. I VAL(FNO)'="@" D CHK^DIE(FILE,FNO,"",VAL(FNO),.RESULT,.ERR)
E1	. I RESULT=U D  Q
	.. S MSG=$G(ERR("DIERR",1,"TEXT",1),"Failed FileMan data validation")
	.. I $L(OUT) S OUT=OUT_"~"
	.. I TFLG!GTFLG S RFLG=1,OUT=FNO_"|"_MSG Q
	.. S OUT=OUT_FNO_"|"_MSG
	.. Q
	. S VAL(FNO)=RESULT
	. I $L(DR) S DR=DR_";"
	. I RESULT="@" S DR=DR_FNO_"////@" Q  ; DELETE THIS VALUE
	. S DR=DR_FNO_"////^S X=VAL("_FNO_")" ; BUILD DR STRING
	. Q
	I $G(RFLG) D:UFLG="A" DIK(DIE,DA) S OUT="Record update cancelled"_"|"_OUT G EX ; TRANSACTION ROLLBACK FLAG IS SET, ENTRY DELETED (ADD MODE) OR UPDATE CANCELLED (EDIT MODE)
	S %=0 F  S %=$O(DAS(%)) Q:'%  S DA(%)=DAS(%) ; JUST IN CASE THIS IS A MILTIPLE, CREATE THE DA ARRAY
DIE	L +@CREF@(DA):2 I $T D ^DIE L -@CREF@(DA) G:OUT["valid" EX S OUT="OK" S:UFLG="A" OUT=OUT_"|"_DA G EX ; SUCCESS!!!!
	S OUT="Update cancelled. File locked" ; FILE LOCKED. UNABLE TO UPDATE
	I $L(FLD),UFLG="A" D DIK(DIE,DA) ; ROLLBACK THE NEW RECORD
EX	D ^XBFMK ; CLEANUP
	Q
	;
REF(FILE,DAS)	; GIVEN A FILE/SUBFILE NUMBER & DAS ARRAY, RETURN THE FM GLOBAL REFERENCE INFO: OREF|CREF|IENS
	N OREF,CREF,IENS,I,X
	S IENS=$$IENS^DILF(.DAS) I '$L(IENS) Q ""
	S OREF=$$ROOT^DILFD(FILE,IENS) I '$L(OREF) Q ""
	S CREF=$$CREF^DILF(OREF) I '$L(CREF) Q ""
	Q (OREF_"|"_CREF_"|"_IENS)
	; 
POINT(FILE,FNO,VAL)	; ADD ACCENT GRAV IF NECESSARY
	I $E(VAL)="`" Q VAL
	I $P($G(^DD(FILE,FNO,0)),U,2)["P",VAL=+VAL,VAL\1=VAL S VAL="`"_VAL
	Q VAL
	; 
WP(FILE,FLD)	; RETURN THE SUBFILE NUMBER IF IT IS A WORD PROCESSING FIELD
	N SF,DTYPE
	S SF=$P($G(^DD(+$G(FILE),+$G(FLD),0)),U,2) I 'SF Q 0
	S DTYPE=$P($G(^DD(SF,.01,0)),U,2)
	I DTYPE["W" Q SF
	Q 0
	;
WORD(FILE,DA,FLD,CREF,VAL)	; SUFF TEXT ENTRY INTO THE WP MULTIPLE FIELD
	N SS,TOT,A,B,I
	S SS=+$P($G(^DD(FILE,FLD,0)),U,4) I SS="" Q
	I VAL="@"!(VAL="") K @CREF@(DA,SS) Q  ; DELETE THE WP RECORD: REMOVE DICTIONARY NODE AND DATA
	S TOT=0
	F  Q:'$L(VAL)  D
	. S A=$E(VAL,1,80),VAL=$E(VAL,81,999999) ; PEEL OFF AN 80 CHARACTER DATA BLOCK FROM THE FRONT OF THE TEXT STRING
	. I $L(A) S TOT=TOT+1,B(TOT)=A ; BUILD THE TEMP ARRAY
	. Q
	I '$D(B(1)) Q  ; NOTHING TO STORE SO QUIT
	S @CREF@(DA,SS,0)="^^"_TOT_U_TOT_U_DT ; SET DICTIONARY NODE
	F I=1:1:TOT S @CREF@(DA,SS,I,0)=B(I) ; SET DATA NODES
	Q
	;
MERR	; MUMPS ERROR TRAP
	N ERR,X
	X ("S X=$"_"ZE")
	S ERR="M ERROR: "_X
	S ^GREG("ERR")=ERR
	S OUT=ERR
	Q
	; 
