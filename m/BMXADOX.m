BMXADOX	; IHS/CIHA/GIS - RPC CALL: GENERATE AN ADO SCHEMA STRING AND DATA SET ;
	;;2.31;BMX;;Jul 25, 2011
	; EXMAPLES OF RPMS SCHEMAE GENERATION
	;
	;
DISP(OUT)	;EP - TEMP DISPLAY
	N I,X
	S I=0 W !
	F  S I=$O(@OUT@(I)) Q:'I  S X=@OUT@(I) S X=$TR(X,$C(30),"}") S X=$TR(X,$C(31),"{") W !,X
	Q
	;
SCHEMA(NAME)	; GIVEN SCHEMA NAME, RETURN THE IEN
	N IEN
	S IEN=$O(^BMXADO("B",NAME,0))
	Q IEN
	;
NEXTNUM(DFN,LOC)	; RETURN THE NEXT PROBLEM NUMBER FOR A PATIENT
	N X,LAST,MAX,NUM
	S NUM=0,MAX=""
	F  S NUM=$O(^AUPNPROB("AA",DFN,LOC,NUM)) Q:NUM=""  S X=$E(NUM,2,99) I +X>MAX S MAX=+X
	I 'MAX Q 1
	S X=X+1 S X=X\1
	Q X
	;
DEMOG	; VIEW DEMOGRAPHICS
	N OUT,%,DFN,MAX,SIEN
	S DFN=1,MAX=1000
	S SIEN=$$SCHEMA("UPDATE PATIENT DEMOGRAPHICS")
	D SS^BMXADO(.OUT,SIEN,"",("~"_DFN_"~"_DFN_"~"_MAX))
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
MEDICARE	; UPDATE MEDICARE DATES/INFO
	N OUT,%,DAS,PIEN,JIEN,DFN,MAX
	S DFN=1,MAX=1000
	S DAS=DFN_","
	S PIEN=$$SCHEMA("UPDATE MEDICARE DATES")
	S JIEN=$$SCHEMA("UPDATE MEDICARE INFO")
	D SS^BMXADO(.OUT,PIEN,DAS,("~"_DFN_"~"_DFN_"~"_MAX_"~~"_"MEDICARE~BMXADOV2~~"_JIEN_",PARENT"))
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
MEDICAID	; VIEW MEDICAID DATES/INFO
	N OUT,%,DAS,PIEN,JIEN,DFN,DA
	S DFN=3
	S DA(1)=$$MCDIEN^BMXADOV2(DFN) I 'DA(1) Q
	S DAS=DA(1)_","
	S PIEN=$$SCHEMA("UPDATE MEDICAID DATES")
	S JIEN=$$SCHEMA("UPDATE MEDICAID INFO")
	D SS^BMXADO(.OUT,PIEN,DAS,("~~~~~MEDICAID~BMXADOV2~~"_JIEN_",PARENT"))
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	; 
PVTINS	; VIEW PRIVATE INSURANCE DATES/INFO
	N OUT,%,DAS,SIEN,DFN
	S DFN=1
	S DAS=DFN_","
	S SIEN=$$SCHEMA("UPDATE PVT INSURANCE INFO")
	D SS^BMXADO(.OUT,SIEN,DAS,"~~~~~PVTINS~BMXADOV2~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	; 
VISIT	; VIEW VISITS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VISITS")
	D SS^BMXADO(.OUT,SIEN,"","AA~3/21/1985~6/4/1986~100~~~~1|R")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
DUPVIS	; DISPLAY POSSIBLE DUPLICATE VISITS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VISITS")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~DUPV~BMXADOV2~1|4/19/04@1PM|I|4585|A~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDVIS	; ADD A NEW VISIT
	N OUT,%,SIEN,DFN,NODE
	S DFN=3
	S SIEN=$$SCHEMA("VISITS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^JUN 03, 2004@09:32^I^`3^`4585^A^`1"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	; 
POV	; DISPLAY POVS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW POVS")
	D SS^BMXADO(.OUT,SIEN,"","AA~3/21/1985~6/4/1986~100~~~~1|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
FLDS	; GET FILEMAN FIELDS
	N OUT,%,SIEN,DFN
	S SIEN=$$SCHEMA("FIELDS")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~FLDIT~BMXADOS1~3.7~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
FINFO	; GET FILEMAN FILEINFO
	N OUT,%,SIEN,DFN
	S SIEN=$$SCHEMA("FILEMAN FILEINFO")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~FNIT~BMXADOS1~3.7~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	; 
ADDPOV	; ADD A POV TO AN EXISITING VISIT
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE POVS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`8718^`1^`71164^DM II ON NEW MEDS^2^P"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
EDITPOV	; ADD A POV TO AN EXISITING VISIT
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE POVS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="100123^`8718^`1^`71164^DM II ON SPECIAL MEDS^2^P"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	; 
PROB	; DISPLAY PROBLEMS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW PROBLEMS")
	D SS^BMXADO(.OUT,SIEN,"","AA~"_DFN_"~"_DFN_"~~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDPROB	; ADD A PROBLEM TO THE PROBLEM LIST
	N OUT,%,SIEN,DFN,NODE,NUM,LOC,ICD,TEXT,AIR,IEN
	S ICD=2477
	S TEXT="HYPERTENSION ON SPECIAL MEDS"
	S DFN=1,LOC=DUZ(2),AIR="A"
	S SIEN=$$SCHEMA("UPDATE PROBLEMS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)=U_"`"_ICD_U_"`"_DFN_U_DT_U_U_TEXT_U_"`"_LOC_U_DT_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	S IEN=+$P(OUT(1),"|",2) I '$D(^AUPNPROB(IEN,0)) Q
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	K OUT
	S NUM=$$NEXTNUM(DFN,LOC) I 'NUM Q  ; PROBLEM NUMBER & STATUS MUST BE ADDED SEPARATELY
	S SIEN=$$SCHEMA("UPDATE PROBLEM NUMBER")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)=IEN_U_NUM_U_"A"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	; 
MEAS	; DISPLAY MEASUREMENTS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW MEASUREMENTS")
	D SS^BMXADO(.OUT,SIEN,"","AA~3/21/1985~6/4/1986~10~~~~"_DFN_"|WT|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDMEAS	; UPDATE V MEASUREMENT FILE
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE MEASUREMENTS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`2^`"_DFN_"^`71164^177.5^`6"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	; 
MEDS	; DISPLAY MEDS
	N OUT,%,SIEN,DFN
	S DFN=3
	S SIEN=$$SCHEMA("VIEW MEDS")
	D SS^BMXADO(.OUT,SIEN,"","AA~1/1/1989~12/31/1990~10~~~~"_DFN_"|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDMEDS	; UPDATE V MED FILE
	N OUT,%,SIEN,DFN,NODE
	S DFN=3
	S SIEN=$$SCHEMA("UPDATE MEDS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`305^`"_DFN_"^`71164^T1T QID^40"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
LAB	; DISPLAY LAB TEST RESULTS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW LABS")
	D SS^BMXADO(.OUT,SIEN,"","AA~1/1/1985~12/31/1987~10~~~~"_DFN_"|175|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDLAB	; UPDATE V LAB
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE LABS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`175^`"_DFN_"^`71164^216"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
EXAMS	; DISPLAY EXAMS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW EXAMS")
	D SS^BMXADO(.OUT,SIEN,"","AA~1/1/1986~12/31/1990~10~~~~"_DFN_"|6|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDEXAMS	; UPDATE V EXAM
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE EXAMS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`6^`"_DFN_"^`71164^NORMAL"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
IMM	; DISPLAY IMMUNIZATIONS
	N OUT,%,SIEN,DFN
	S DFN=2
	S SIEN=$$SCHEMA("VIEW IMM")
	D SS^BMXADO(.OUT,SIEN,"","AA~1/1/1986~12/31/1988~10~~~~"_DFN_"|12|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDIMM	; UPDATE V IMMUNIZATION FILE
	S DFN=2
	S SIEN=$$SCHEMA("UPDATE IMM")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`12^`"_DFN_"^`71164^2"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
PROV	; DISPLAY PROVIDERS FOR A VISIT
	N OUT,%,SIEN,VIEN
	S VIEN=11
	S SIEN=$$SCHEMA("VIEW PROV")
	D SS^BMXADO(.OUT,SIEN,"","AD~"_VIEN_"~"_VIEN_"~10~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDPROV	; UPDATE V PROVIDER FILE
	N OUT,%,SIEN,NODE,PIEN,DFN
	S PIEN=5,DFN=1
	I $P(^DD(9000010.06,.01,0),U,3)["DIC(6" S PIEN=$P(^VA(200,PIEN,0),U,16) ; CONVERT FILE 200 TO FILE 16 IF NECESS.
	S SIEN=$$SCHEMA("UPDATE PROV")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`"_PIEN_"^`"_DFN_"^`71164^P"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
PROC	; DISPLAY PROCEDURES
	N OUT,%,SIEN,DFN
	S DFN=4
	S SIEN=$$SCHEMA("VIEW PROCEDURES")
	D SS^BMXADO(.OUT,SIEN,"","AA~1/1/1985~12/31/1985~10~~~~"_DFN_"|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDPROC	; UPDATE V PROCEDURES FILE
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE PROCEDURES")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`2198^`"_DFN_"^`71164^`8718"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
CPT	; DISPLAY CPT CODES
	N OUT,%,SIEN,DFN
	S VIEN=71164
	S SIEN=$$SCHEMA("VIEW CPT")
	D SS^BMXADO(.OUT,SIEN,"","AD~"_VIEN_"~"_VIEN_"~10~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDCPT	; UPDATE V CPT FILE
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE CPT")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`10000^`"_DFN_"^`71164^WOUND CARE"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
PH	; DISPLAY PERSONAL HISTORY
	N OUT,%,SIEN,DFN
	S DFN=632
	S SIEN=$$SCHEMA("VIEW PERSONAL HISTORY")
	D SS^BMXADO(.OUT,SIEN,"","AC~"_DFN_"~"_DFN_"~~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDPH	; UPDATE PERSONAL HX
	N OUT,%,SIEN,DFN,NODE,ICD,TEXT
	S ICD=2477
	S TEXT="PERSONAL HISTORY OF SERIOUS PROBLEMS"
	S DFN=632
	S SIEN=$$SCHEMA("UPDATE PERSONAL HISTORY")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`11353^`"_DFN_"^2851219^"_TEXT_"^2810303"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
FH	; DISPLAY FAMILY HX
	N OUT,%,SIEN,DFN
	S DFN=631
	S SIEN=$$SCHEMA("VIEW FAMILY HISTORY")
	D SS^BMXADO(.OUT,SIEN,"","AC~"_DFN_"~"_DFN_"~~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDFH	; UPDATE FAMILY HISTORY
	N OUT,%,SIEN,DFN,NODE,ICD,TEXT
	S ICD=2477
	S TEXT="FAMILY HISTORY OF SERIOUS PROBLEMS"
	S DFN=631
	S SIEN=$$SCHEMA("UPDATE FAMILY HISTORY")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`7571^`"_DFN_"^2851219^"_TEXT_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	; 
HF	; DISPLAY HEALTH FACTORS
	N OUT,%,SIEN,DFN
	S DFN=1
	S SIEN=$$SCHEMA("VIEW HEALTH FACTORS")
	D SS^BMXADO(.OUT,SIEN,"","AC"_"~"_DFN_"~"_DFN_"~~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	; 
ADDHF	; UPDATE HEALTH FACTORS FILE
	N OUT,%,SIEN,DFN,NODE
	S DFN=1
	S SIEN=$$SCHEMA("UPDATE HEALTH FACTORS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`3^`"_DFN_U_DT_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
REPRO	; DISPLAY REPRODUCTIVE FACTORS
	N OUT,%,SIEN,DFN
	S DFN=5
	S SIEN=$$SCHEMA("VIEW REPRODUCTIVE FACTORS")
	D SS^BMXADO(.OUT,SIEN,"","B"_"~"_DFN_"~"_DFN_"~~~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ADDREPRO	; UPDATE REPRODUCTIVE FACTORS
	; THE .O1 FIELD IS DINUMED
	; THEREFORE, THE FILER WILL AUTOMATICALLY SWITCH TO MOD MODE IF A RECORD ALREADY EXISTS FOR THIS PATIENT
	N OUT,%,SIEN,DFN,NODE
	S DFN=5
	; I $D(^AUPNREP(DFN)) G ERF
	S SIEN=$$SCHEMA("ADD REPRODUCTIVE FACTORS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`"_DFN_"^G5P4LC3SA1TA0^"_DT_"^2^3040101^"_DT_$C(30)
	D DISP(OUT) R %:$G(DTIME,60)
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J)))
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%)
	Q
	;
	; ----------------------------------  GRIDS  ---------------------------------------------
	;
GRID	; POPULATE THE INTRO GRID
	N OUT,%,SIEN,NODE,NEXT
	S NEXT="70470;0"
	S SIEN=$$SCHEMA("VEN MOJO DE INTRO")
	D SS^BMXADO(.OUT,SIEN,"","ASEG~"_NEXT_"~"_NEXT) ; GET SCHEMA
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
MGRID	; POPULATE THE MEASUREMENT GRID
	N OUT,%,SIEN,NODE,NEXT,START,STOP
	S NEXT="70470;2"
	S SIEN=$$SCHEMA("VEN MOJO DE MEASUREMENT")
	; D SS^BMXADO(.OUT,SIEN,"","~~~~~GRIDIT~VENPCCTG~"_NEXT) ; GET SCHEMA
	D SS^BMXADO(.OUT,SIEN,"","ASEG~"_NEXT_"~"_NEXT) ; GET SCHEMA
	D DISP(OUT) R %:$G(DTIME,60)
	; K ^TMP("BMX ADO",$J)
	Q
	;
PRVGRID	; POPULATE THE PROVIDER GRID
	N OUT,%,SIEN,NODE,NEXT
	S NEXT="70470;4"
	S SIEN=$$SCHEMA("VEN MOJO DE PROVIDER")
	D SS^BMXADO(.OUT,SIEN,"","ASEG~"_NEXT_"~"_NEXT) ; GET SCHEMA
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
CLGRID	; POPULATE THE CLINIC GRID
	N OUT,%,SIEN,NODE,NEXT
	S NEXT="70470;8"
	S SIEN=$$SCHEMA("VEN MOJO DE CLINIC")
	D SS^BMXADO(.OUT,SIEN,"","ASEG~"_NEXT_"~"_NEXT) ; GET SCHEMA
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
DXGRID	; POPULATE THE DX GRID
	N OUT,%,SIEN,NODE,NEXT
	S NEXT="70470;1"
	S SIEN=$$SCHEMA("VEN MOJO DE DX DXHX")
	D SS^BMXADO(.OUT,SIEN,"","ASEG~"_NEXT_"~"_NEXT) ; GET SCHEMA
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
