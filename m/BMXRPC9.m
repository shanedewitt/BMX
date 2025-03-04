BMXRPC9	; IHS/OIT/HMW - RPC CALL FOR EXTENDED BROKER FUNCTIONALITY ;
	;;2.31;BMX;;Jul 25, 2011
	; UPDATE FILEMAN WITH AN ADO RECORD SET FROM A WINDOWS APPLICATION
	;
	;
	;
SONLY(BMXY,BMXVAL)	;EP Schema Only
	;
	I BMXVAL="TRUE" S BMX("SCHEMA ONLY")=1
	E  S BMX("SCHEMA ONLY")=0
	S BMXY=BMX("SCHEMA ONLY")
	;
	Q
	;
TESTRPC(BMXGBL,BMXSQL)	;
	;Test retrieval/update statement
	;
	N BMXI,BMXERR,BMXN,BMXNOD,BMXNAM,BMXSEX,BMXDOB,BMXFAC,BMXTMP,BMXJ
	S X="ETRAP^BMXRPC9",@^%ZOSF("TRAP")
	S BMXGBL="^BMXTMP("_$J_")",BMXERR="",U="^"
	K ^BMXTMP($J)
	S BMXI=0
	;
	;Old column info format:
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="I00010BMXIEN"_U_"D00010DOB"_U_"T00030LOCAL_FACLILITY"_U_"T00030NAME"_U_"T00010SEX"_$C(30)
	;
	;New column info format is @@@meta@@@KEYFIELD|FILE#
	;   For each field: ^FILE#|FIELD#|DATATYPE|LENGTH|FIELDNAME|READONLY|KEYFIELD|NULL ALLOWED
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="@@@meta@@@"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="BMXIEN|2160010^"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="2160010|.001|I|10|BMXIEN|TRUE|TRUE^"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="2160010|.03|D|10|DOB|FALSE|FALSE^"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="2160010|.04|T|60|LOCAL_FACILITY|FALSE|FALSE^"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="2160010|.01|T|30|NAME|FALSE|FALSE^"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)="2160010|.02|T|10|SEX|FALSE|FALSE"
	;S BMXI=BMXI+1,^BMXTMP($J,BMXI)=$C(30)
	;
	D SS^BMXADO(.BMXTMP,"","TEST1")
	I $G(BMXTMP)=$C(30) D ERR(99,"SCHEMA GENERATION FAILED") Q
	S BMXJ=0 F  S BMXJ=$O(BMXTMP(BMXJ)) Q:'+BMXJ  D
	. S BMXI=BMXI+1
	. S ^BMXTMP($J,BMXI)=BMXTMP(BMXJ)
	I +$G(BMX("SCHEMA ONLY")) D  Q
	. S BMXI=BMXI+1
	. S ^BMXTMP($J,BMXI)=$C(31)
	. Q
	S BMXN=0
	F  S BMXN=$O(^DIZ(2160010,BMXN)) Q:'+BMXN  D
	. Q:'$D(^DIZ(2160010,BMXN,0))
	. S BMXNOD=^DIZ(2160010,BMXN,0)
	. S BMXNAM=$P(BMXNOD,U)
	. S BMXSEX=$P(BMXNOD,U,2)
	. S BMXDOB=$P(BMXNOD,U,3)
	. S Y=BMXDOB X ^DD("DD") S BMXDOB=Y
	. S BMXFAC=$P(BMXNOD,U,4)
	. S:+BMXFAC BMXFAC=$P($G(^DIC(4,BMXFAC,0)),U)
	. S BMXI=BMXI+1
	. S ^BMXTMP($J,BMXI)=BMXN_U_BMXDOB_U_BMXFAC_U_BMXNAM_U_BMXSEX_$C(30)
	. Q
	S BMXI=BMXI+1
	S ^BMXTMP($J,BMXI)=$C(31)
	Q
	;
ERR(BMXID,BMXERR)	;Error processing
	K ^BMXTMP($J)
	S ^BMXTMP($J,0)="I00030ERRORID^T00030ERRORMSG"_$C(30)
	S ^BMXTMP($J,1)=BMXID_"^"_BMXERR_$C(30)
	S ^BMXTMP($J,2)=$C(31)
	Q
	;
ETRAP	;EP Error trap entry
	D ^%ZTER
	D ERR(99,"BMXRPC9 Error: "_$G(%ZTERROR))
	Q
	;
TEST	N OUT S OUT="" D ADO(.OUT,2160010,"1",(".01|A,A"_$C(30)_".02|M"_$C(30)_".03|1/5/1946"_$C(30)_".04|SAN XAVIER"_$C(31))) W !,OUT
	Q
	;
ADOX(OUT,FILE,IEN,DATA)	;
	;
	D DEBUG^%Serenji("ADOX^BMXRPC9(.OUT,FILE,IEN,DATA)")
	;
	Q
	;
ADO(OUT,FILE,IEN,DATA)	; RPC CALL: OUT = OUTBOUND MESSAGE, FILE = FILEMAN FILE NUMBER, IEN = FILE INTERNAL ENTRY NUMBER, DATA = DATA STRING
	N OREF,CREF,DIC,DIE,DA,DR,X,Y,%,FLD,CNT,FNO,VAL,TFLG,DFLG,TOT,UFLG,XTFLG,GTFLG,GDFLG
	S OUT="",FLD="",GTFLG=0,GDFLG=0
	S IEN=$G(IEN)
	I $E(IEN)="-" S IEN=$E(IEN,2,99),GDFLG=1 ; GLOBAL DELETE FLAG
	I $E(IEN)="+" S IEN=$E(IEN,2,99),GTFLG=1 ; GLOBAL TRANSACTION FLAG, ROLLBACK IF ANY FIELD FAILS TO UPDATE
	I IEN="Add"!(IEN="ADD") S IEN=""
	I '$D(^DIC(+$G(FILE),0,"GL")) S OUT="Update cancelled. Invalid FILE number" Q
	S OREF=^DIC(FILE,0,"GL") I '$L(OREF) S OUT="Update cancelled.  Invalid file definition" Q
	S CREF=$E(OREF,1,$L(OREF)-1) I $E(OREF,$L(OREF))="," S CREF=CREF_")" ; CONVERT OREF TO CREF
	I IEN,'$D(@CREF@(IEN)) S OUT="Update cancelled. Invalid IEN" Q
	I 'GDFLG,IEN,(DATA["-.01|"!(DATA[".01|@")) S GDFLG=1
	I GDFLG,'IEN S OUT="Deletion cancelled. Missing IEN" Q
	I GDFLG D DIK(OREF,IEN) S OUT="Record deleted|"_IEN Q
	S UFLG=$S($G(IEN):"E",1:"A") ; UPDATE FLAG: ADD OR EDIT
	I '$L($G(DATA)) S OUT="Update cancelled.  Missing/invalid data string" Q
	S TOT=$L(DATA,$C(30)) I 'TOT S OUT="Update cancelled.  Missing data string" Q
	F CNT=1:1:TOT S DATA(CNT)=$P(DATA,$C(30),CNT) ; BUILD PRIMARY FIELD ARRAY
	S %=DATA(1) I %=""!(%=$C(31)) S OUT="Update cancelled.  Missing data string" Q
	S %=DATA(CNT) I %[$C(31) S %=$P(%,$C(31),1),DATA(CNT)=% ; STRIP OFF END OF FILE MARKER
	F CNT=1:1:TOT S X=DATA(CNT) I $L(X) D  ; BUILD SECONDARY FIELD ARRAY
	. S TFLG=0,DFLG=0
	. I $E(X)="+" S TFLG=1,X=$E(X,2,999),$P(FLD,U)=1
	. I $E(X)="-" S DFLG=1,X=$E(X,2,999)
	. S FNO=$P(X,"|"),VAL=$P(X,"|",2)
	. I '$D(^DD(FILE,+$G(FNO),0)) S:$L(OUT) OUT=OUT_"~" S OUT=OUT_FNO_"|Invalid field number" Q
	. I DFLG,VAL'="" S:$L(OUT) OUT=OUT_"~" S OUT=OUT_FNO_"|Invalid deletion syntax" Q  ; CANT DELETE IF A VALUE IS SENT
	. I DFLG!(VAL="") S VAL="@" ; SYNC DFLG AND VAL
	. I VAL="@" S DFLG=1 ; SYNC DFLG AND VAL
	. S FLD(FNO)=VAL_U_TFLG_U_DFLG
	. I FNO=.01,TFLG S $P(FLD,U,2)=1 ; 
	. Q
	I $P($G(FLD(.01)),U,3),UFLG="A" S OUT="Record deletion cancelled.  Missing IEN" Q  ; CAN'T DELETE A RECORD WITHOUT A VALID IEN
DELREC	I $P($G(FLD(.01)),U,3) D DIK(OREF,IEN) S OUT="OK" Q  ; DELETE THE RECORD
	I UFLG="A",'$L($P($G(FLD(.01)),U)) S OUT="Record addition cancelled.  Missing .01 field" Q  ; CAN'T ADD A RECORD WITHOUT A VALID .01 FIELD
ADDREC	I UFLG="A" D ADD(OREF) Q  ; ADD A NEW ENTRY TO A FILE
EDITREC	I UFLG="E" D EDIT(OREF,IEN) Q  ; EDIT AN EXISTING RECORD
	Q
	;
DIK(DIK,DA)	; DELETE A RECORD
	D ^DIK
	D ^XBFMK
	Q
	;
ADD(DIC)	; ADD A NEW ENTRY TO A FILE
	N X,Y
	S X=""""_$P($G(FLD(.01)),U)_""""
	S DIC(0)="L"
	D ^DIC
	I Y=-1 S OUT="Unable to add a new record" G AX
	I $O(FLD(.01)) D EDIT(DIC,+Y) Q
	S OUT="OK"_"|"_+Y
AX	D ^XBFMK
	Q
	;
EDIT(DIE,DA)	; EDIT AN EXISTING RECORD
	N DR,RFLG,ERR,FNO,VAL,TFLG,RESULT,MSG,DIERR,DISYS
	S FNO=$O(FLD(.01),-1),DR="" ;HMW Changed to include .01 in DR string
	I UFLG="A" S OUT="New record added|"_DA
	F  S FNO=$O(FLD(FNO)) Q:'FNO  S X=FLD(FNO) I $L(X) D  I $G(RFLG) Q  ; CHECK EA FIELD AND BUILD THE DR STRING AND ERROR STRING
	. S VAL(FNO)=$P(X,U),TFLG=$P(X,U,2) I '$L(VAL(FNO)) Q
	. K ERR,RESULT
	. I VAL(FNO)="@"!(VAL(FNO)="") S RESULT="@"
	. E  D CHK^DIE(FILE,FNO,"",VAL(FNO),.RESULT,"ERR")
	. I RESULT=U D  Q
	.. S MSG=$G(ERR("DIERR",1,"TEXT",1),"Failed FileMan data validation")
	.. I $L(OUT) S OUT=OUT_"~"
	.. I TFLG!GTFLG S RFLG=1,OUT=FNO_"|"_MSG Q
	.. S OUT=OUT_FNO_"|"_MSG
	.. Q
	. S VAL(FNO)=RESULT
	. I $L(DR) S DR=DR_";"
	. S DR=DR_FNO_"////^S X=VAL("_FNO_")" ; BUILD DR STRING
	. Q
	I $G(RFLG) D:UFLG="A" DIK(DIE,DA) S OUT="Record update cancelled"_"|"_OUT G EX ; TRANSACTION ROLLBACK FLAG IS SET, ENTRY DELETED (ADD MODE) OR UPDATE CANCELLED (EDIT MODE)
	L +@CREF@(DA):2 I $T D ^DIE L -@CREF@(DA) G:OUT["valid" EX S OUT="OK" S:UFLG="A" OUT=OUT_"|"_DA G EX ; SUCCESS!!!!
	S OUT="Update cancelled. File locked" ; FILE LOCKED. UNABLE TO UPDATE
	I $L(FLD),UFLG="A" D DIK(DIE,DA) ; ROLLBACK THE NEW RECORD
EX	D ^XBFMK ; CLEANUP
	Q
	; 
