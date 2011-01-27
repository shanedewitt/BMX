BMXADOXY	; IHS/CIHA/GIS - RPC CALL: GENERATE AN ADO SCHEMA STRING AND DATA SET ;
	;;2.3;BMX;;Jan 25, 2011
	; EXMAPLES OF FILEMAN SCHEMA GENERATION
	;
	;
	;
DISP(OUT)	; TEMP DISPLAY OF THE ANR
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
NUM	; ITERATE BY IEN
	; IX="",START WITH IEN=1, STOP AFTER IEN=20, MAX # RECORDS RETURNED = 5
	; TO VIEW INTERNAL VALUES SET VSTG="~1~20~5~I"
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("IHS PATIENT")
	D SS^BMXADO(.OUT,SIEN,"","~1~20~5")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
IX	; ITERATE BY INDEX
	; ITERATE USING THE "B" INDEX
	; START WITH PT NAME "C", STOP AFTER PATIENT NAME = "D", MAX # RECORDS RETURNED = 5
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("IHS PATIENT")
	D SS^BMXADO(.OUT,SIEN,"","B~C~D~5")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
RENT	; ITERATE IN CHUNKS
	; RE-ITERATE USING THE "B" INDEX
	; START WITH PT IEN 5 AS THE "SEED", STOP AFTER PATIENT NAME = "D", MAX # RECORDS RETURNED = 5
	N OUT,%,SIEN,SEED,LSEED,X,Y
	S SEED=0,LSEED=""
	S SIEN=$$SCHEMA("IHS PATIENT")
RIT	F  D  I '$G(SEED) Q
	. D SS^BMXADO(.OUT,SIEN,SEED,"B~CA~CB~5")
	. D DISP(OUT) R %:$G(DTIME,60) E  S SEED="" Q
	. I %?1"^" S SEED="" Q
	. S X=$P(@OUT@(1),U,1)
	. S SEED=$P(X,"|",3)
	. I SEED=LSEED S SEED="" Q
	. S LSEED=SEED
	. K ^TMP("BMX ADO",$J)
	. Q
	Q
	;
SUB	; SUBFILE ITERATION
	; THE SCHEMA IS ATTACHED TO THE MEDICARE ELIGIBILITY FILE/ELIG DATE SUBFILE
	; THE DA STRING HAS A VALUE OF '4,',: THE IEN IN THE PARENT FILE (PATIENT DFN).
	; NOTE THE COMMA IN THE DA STRING.  THIS INDICATES THAT THE FILE IEN IS 4 BUT THE SUBFILE IEN IS UNSPECIFIED 
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("UPDATE MEDICARE DATES")
	;D SS^BMXADO(.OUT,SIEN,"1,","~~~")
	D SS^BMXADO(.OUT,18,"1,","~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
DINUM	; DINUMED POINTER ITERATION
	; THE SCHEMA IS ATTACHED TO THE PATIENT FILE (9000001)
	; THE PATIENT FILE IS DINUM'D AND ITS .01 FIELD POINTS TO THE VA PATIENT FILE (2)
	; BECAUSE OF THE SPECIAL RELATIONSHIP BETWEEN THE FILES, WE CAN USE THE B INDEX OF FILE 2 TO ITERATE FILE 9000001.
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("IHS PATIENT")
	D SS^BMXADO(.OUT,SIEN,"","B~A~B~5")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
IXP	; INDEXED POINTER ITERATION
	; THE SCHEMA IS ATTACHED TO THE V POV FILE
	; THE AC CROSS REFERENCE INDEXES THE PATIENT FIELD
	; BY STARTING AND STOPING WITH PATIENT 235 (MAX=5) WE COLLECT THE FIRST 5 POVS FOR PATIENT 235 IN THE FILE
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("VIEW POVS")
	D SS^BMXADO(.OUT,SIEN,"","AC~235~235~5")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
AA	; ITERATE USING AA INDEX
	; INDEX IS 'AA"  THE START AND STOP DATES ARE SPECIFIED IN EXTERNAL FORMAT.  MAX=10
	; THE FOLLOWING FILTERS ARE SPECIFIED IN THE LAST PARAMETER ("235|WT|C"):
	;   235=PATIENT DFN #235
	;   WT=RETURN ONLY WEIGHTS. MEASUREMENT TYPE MUST BE SPECIFIED WITH A VALID, UNAMBIGUOUS LOOKUP VALUE.
	;   C=RETRUN VALUES IN CHRONOLOGICAL ORDER USE 'R' INSTEAD OF 'C' FOR REVERSE CHRONOLOGICAL ORDER. DEFAULT=C
	;   THE SEED PARAMTER IS SET AND CAN BE USED TO RETURN DATA IN CHUNKS
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("VIEW MEASUREMENTS")
	D SS^BMXADO(.OUT,SIEN,"","AA~3/21/1965~6/4/2004~5~~~~235|WT|C")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
AA2	; ITERATE USING AA INDEX
	; THIS SCHEMA IS ATTACHED TO THE VISIT FILE (9000010)
	; IN THIS CASE THERE IS NO ATTRIBUTE TYPE SO THE FILTER PARAM HAS ONLY 2 PIECES "1|R"
	;   235=PATIENT DFN
	;   R=RETURN DATA IN REVERSE CHRONOLOGICAL ORDER
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("VISITS") ;12
	D SS^BMXADO(.OUT,SIEN,"","AA~3/21/1965~6/4/2004~5~~~~235|R")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
CIT	; CUSTOM ITERATOR
	; IF COMPLEX OR UNUSUAL SORTING/FILTERING IS REQUITED, USE A CUSTOM ITERATOR
	; THE CUSTOM ITERATOR IS DEFINED BY 6TH, 7TH AND 8TH PIECES IN THE VSTG
	;   PIECE 8=TAG, PIECE 9=ROUTINE, PIECE 8=A PARAMETER PASSED TO THE ENTRY POINT
	;   THE 9TH PIECE CONTAINS PT DFN, TIMESTAMP, VISIT TYPE, LOC IEN, AND SERVICE CATEGORY IN A "|" DELIMTED STRING
	;   THE ITERATOR CALL TAG^ROUTINE(PARAM) TO GENERATE IENS
	; IN THIS CASE THE SCHEMA IS ATTACHED TO THE VISIT FILE.
	; GIVEN THE INFORMATION IN THE PARAMETER, THE CUSTOM ITERATOR RETURNS POSSIBLE DUPLICATE VISITS
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("VISITS")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~DUPV~BMXADOV2~9285|5/24/04@1PM|I|516|~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
ID	; IDENTIFIER FIELD
	; THE SCHEMA IS ATTACHED TO THE VA PATIENT FILE (2)
	; THE SCHEMA HAS A BUILT IN FIELD (.01ID) THAT RETURNS THE IDENTIFIERS
	; THE ENTRY POINT THAT GENERATES THE IDETIFIERS IS STORED IN THE BMX ADO SCHEMA FILE
	; PATIENT DFN=235
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("UPDATE PATIENT DEMOGRAPHICS")
	D SS^BMXADO(.OUT,SIEN,"","~235~235~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
JSTD	; STANDARD JOIN
	; BY SPECIFYING A JOIN IN THE VSTG, MULTIPLE SCHEMAE AND DATA SETS ARE RETURNED IN ONE PASS
	; THE SCHEMA IS ATTACHED TO THE V MEASUREMENT FILE
	; THIS IS JOINED TO A SECOND FILE, THE VA PATIENT FILE VIA A JOIN
	; THE JOIN IS BASTED ON THE FACT THAT THE PATIENT FIELD (.02) IN THE V MEASUREMENT FILE POINTS TO THE VA PATIENT FILE
	; THE JOIN PARAMETER IS THE 9TH PIECE OF THE VSTG. IT CONSISTS OF 2 PIECES DELIMITED BY A ","
	;   PIECE 1 IS THE SCHEMA THAT YOU ARE JOINING TO
	;   PIECE 2 IS THE FIELD IN THE PRIMARY FILE THAT ENABLES THE JOIN
	; THE DATA SET FROM THE SECOND (JOIN) FILE CONTAINS ONLY THOSE RECORDS NECESSARY TO COMPLETE THE JOIN
	; PATIENT DFN=235, INDEX=AA, MAX=5, START=3/21/65, STOP=6/4/04
	N OUT,%,SIEN1,SIEN2
	S SIEN2=$$SCHEMA("VIEW MEASUREMENTS")
	S SIEN1=$$SCHEMA("PATIENT DEMOGRAPHICS")
	;SIEN1=23, SIEN2=11
	;D SS^BMXADO(.OUT,SIEN1,"","AA~3/21/1965~6/4/2004~5~~~~234|WT|C~"_SIEN2_",.02")
	D SS^BMXADO(.OUT,SIEN1,"","~234~236~~~~~~"_SIEN2_",.01")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
HWSTD	;
	; PATIENT DFN=235, INDEX=AA, MAX=5, START=3/21/65, STOP=6/4/04
	N OUT,%,SIEN1,SIEN2
	S SIEN1=$$SCHEMA("PATIENT DEMOGRAPHICS")
	S SIEN2=$$SCHEMA("VIEW MEASUREMENTS")
	;SIEN2=23, SIEN1=11
	D SS^BMXADO(.OUT,SIEN1,"","~235~250~~~~~~"_SIEN2_",.01")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
JMD	;JOIN MASTER TO DETAIL
	N OUT,%,SIEN1,SIEN2,SIEN3,VSTG
	S SIEN1=$$SCHEMA("PATIENT DEMOGRAPHICS")
	S SIEN2=$$SCHEMA("VIEW MEASUREMENTS")
	S SIEN3=$$SCHEMA("VIEW MEDS")
	S VSTG="~1~5~~~~~~"
	;S VSTG=VSTG_SIEN3_",.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|C"
	S VSTG=VSTG_SIEN3_",.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|C"
	;S VSTG="~1~5~~~~~~23,.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|WT|C"
	;BMX ADO SS^11^^~1~5~~~~~~23,.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|WT|C
	;BMX ADO SS^11^^~1~5~~~~~~25,.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|C
	D SS^BMXADO(.OUT,SIEN1,"",VSTG)
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
JSUB	; SUBFILE JOIN
	; IN THIS CASE THE RECORDS IN A PARENT FILE ARE "JOINED" TO THE RECORDS IN ONE OF ITS SUB FILES
	; THE SCHEMA IS ATTACHED TO THE "MEDICARE ELIGIBLE" FILE
	; IT IS JOINED TO ITS SUBFILE, "ELIG DATES", VIA THE UPDATE MEDICARE DATES SCHEMA
	; THE SYNTAX FOR THE JOIN PIECE IS "sien2,SUB" WHERE sien2=IEN OF SECOND SCHEMA
	; PATIENT DFN=4
	N OUT,%,SIEN1,SIEN2
	S SIEN1=$$SCHEMA("UPDATE MEDICARE INFO") ;17
	S SIEN2=$$SCHEMA("UPDATE MEDICARE DATES") ;18
	;BMX ADO SS^17^^~4~5~~~~~~18,SUB
	D SS^BMXADO(.OUT,SIEN1,"","~4~5~~~~~~"_SIEN2_",SUB")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
JPAR	; PARENT FILE JOIN
	; SIMILAR TO A SUBFILE JOIN EXCEPT THE SUB-FILE IS TREATED AS THE PRIMARY FILE AND IT IS JOINED TO ITS PARENT
	; BECAUSE WE ARE STARTING IN A SUBFILE, THE DA STRING CONTAINS THE IEN OF THE PARENT FILE ("4,"
	; THE SYNTAX OF THE 9TH PIECE IS "sien2,PARENT" WHERE sien2 IS THE IEN OF THE SECONDARY SCHEMA
	; PATIENT DFN=4
	N OUT,%,SIEN1,SIEN2
	S SIEN1=$$SCHEMA("UPDATE MEDICARE DATES")
	S SIEN2=$$SCHEMA("UPDATE MEDICARE INFO")
	D SS^BMXADO(.OUT,SIEN1,"4,","~~~5~~~~~"_SIEN2_",PARENT")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	; 
ADD	; ADD A NEW ENTRY
	; THIS IS A 2 STEP PROCESS:
	;   FIRST GET THE SCHEMA FOR THE FILE YOU WISH TO UPDATE
	;     THIS SCHEMA'S NAME TYPICALLY BEGINS WITH THE WORD "UPDATE"
	;     IT CONTAINS NO ID OR IEN FIELDS
	;   SECOND ADD THE DATA NODE TO THE ARRAY
	;     IT HAS THE SAME FORMAT AS A DATA STRING ASSOCIATED WITH THE SCHEMA EXCEPT THE FIRST "^" PIECE IS NULL
	;     THIS PIECE CORRESPONDS TO THE IEN OF THE RECORD. SINCE THE RECORD HAS NOT BEEN ADDED YET, IT IS NULL.
	;     IN THE DATA STRING, ALL POINTER VALUES ARE PRECEDED BY THE '`' CHARACTER AND EA. STRING ENDS IN $C(30)
	;     MULTIPLE DATA STRINGS CAN BE APPENDED AS NEW NODES AT THE BOTTOM OF THE ARRAY
	;     IN THIS CASE WE ARE ADDING A RECORD TO THE V MEASUREMENT FILE
	;     DATA STRING="^MEASUREMENT TYPE IEN^PATIENT DFN^VISIT IEN^RESULT"_$C(30)
	; THERE ARE 2 INPUT PARAMS:
	;   THE CLOSED REF WHERE THE INPUT ARRAY IS STORED
	;   SINCE IT IS PASSED BY REFERENCE "OUT" CAN BE NULL OR UNDEFIEND.
	;   OUT WILL BE DEFINED AT THE CONCLUSION OF THE TRANSACTION.
	; THE OUTPUT IS IN THE OUT ARRAY
	;   OUT(1)="OK|ien" WHERE ien IS THE IEN OF THE RECORD THAT HAS BEE ADDED.
	;   IF THE TRANSACTION FAILED, AN ERROR MSG WILL BE IN THE OUT ARRAY
	; MEASUREMENT TYPE=2, PATIENT DFN=2, VISIT IEN=7806, PATIENT'S WEIGHT=172.75
	N OUT,%,SIEN,NODE,DFN
	S DFN=2
	S SIEN=$$SCHEMA("UPDATE MEASUREMENTS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="^`2^`"_DFN_"^`7806^172.75"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60) ; DISPLAY THE INPUT ARRAY BEFORE UPDATING THE RECORD
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J))) ; EP FOR UPDAING THE RECORD
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%) ; SEND BACK AN ACKNOWLEDGEMENT OR ERROR MSG
	Q
	;
DEL	; DELETE A RECORD
	; THE SIPLEST WAY TO DELETE AN ENTRY IS TO PUT THE RECORD IEN IN THE DA STRING PRECEDED BY A MINUS SIGN
	; YOU CAN ALSO SET THE VALUE OF THE .01 FIELD TO "@"
	; IF THE VALUE OF THE .01 FIELD IS NULL AND THE DA STRING IS NOT PRECEDED BY A MINUS SIGN, THE TRANSACTION WILL BE CANCELLED
	; IF THE DA STRING IS NULL, THE TRANSACTION WILL BE CANCELLED
	; IN THIS EXAMPLE, WE DELETE A V MEASUREMENT RECORD THAT WAS JUST ADDED
	N OUT,%,SIEN,NODE,DEL
	S DEL=1621
	S SIEN=$$SCHEMA("UPDATE MEASUREMENTS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="-"_DEL_$C(30)
	D DISP(OUT) R %:$G(DTIME,60) ; DISPLAY THE INPUT ARRAY BEFORE UPDATING THE RECORD
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J))) ; EP FOR UPDAING THE RECORD
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%) ; SEND BACK AN ACKNOWLEDGEMENT OR ERROR MSG
	Q
	;
EDIT	; EDIT AN EXISTING ENTRY
	; SIMILAR TO ABOVE EXCEPT THAT THE FIRST "^" PIECE OF THE DATA NODE IS THE IEN OF THE RECORD TO BE EDITIED
	; NOTE THAT THERE IS NO '`' IN FRONT OF THE FIRST PIECE.  IT IS A PURE INTEGER
	; LAB TEST=175, PATIENT DFN=2, VISIT IEN=8040, PT'S GLUCOSE=276, ANORMAL="ABNORMAL"
	N OUT,%,SIEN,NODE
	S SIEN=$$SCHEMA("UPDATE LABS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="279^`175^`2^`8040^280^H"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60) ; DISPLAY THE INPUT ARRAY BEFORE UPDATING THE RECORD
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J))) ; EP FOR UPDAING THE RECORD
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%) ; SEND BACK AN ACKNOWLEDGEMENT OR ERROR MSG
	Q
	;
DELVAL	; DELETE A VALUE IN A FIELD
	; SIMILAR TO EDIT EXCEPT THE VALUE IS "@"
	; DELETE WILL BE ABORTED IF IF FILEMAN SAYS THIS IS A REQUIRED FIELD
	N OUT,%,SIEN,NODE
	S SIEN=$$SCHEMA("UPDATE LABS")
	D SS^BMXADO(.OUT,SIEN,"","") ; GET SCHEMA
	S NODE=$O(^TMP("BMX ADO",$J,999999),-1)+1
	S ^TMP("BMX ADO",$J,NODE)="279^`175^`2^`8040^^@"_$C(30)
	D DISP(OUT) R %:$G(DTIME,60) ; DISPLAY THE INPUT ARRAY BEFORE UPDATING THE RECORD
	D BAFM^BMXADOF1(.OUT,$NA(^TMP("BMX ADO",$J))) ; EP FOR UPDAING THE RECORD
	K ^TMP("BMX ADO",$J)
	W !!,OUT S %=0 F  S %=$O(OUT(%)) Q:'%  W !,OUT(%) ; SEND BACK AN ACKNOWLEDGEMENT OR ERROR MSG
	Q
	;
