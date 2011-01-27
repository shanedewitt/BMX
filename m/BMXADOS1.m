BMXADOS1	; IHS/CIHA/GIS - UPDATE THE BMX ADO SCHEMA FILE GUI VERSION ;
	;;2.3;BMX;;Jan 25, 2011
	; RPC CALLS
	; 
	; 
	;
DISP(OUT)	; TEMP DISPLAY
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
FILE	; RETURN A LIST OF FILES
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("FILEMAN FILES")
	D SS^BMXADO(.OUT,SIEN,"","B~B~C~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
SF	; RETURN A LIST OF SUBFILES
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("SUBFILES")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~SFIT~BMXADOS1~2~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
FLD	; RETURN LIST OF FIELDS FOR A FILE OR SUBFILE
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("FIELDS")
	D SS^BMXADO(.OUT,SIEN,"","~~~~~FLDIT~BMXADOS1~2~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
SCH	; RETURN A LIST OF SCHEMAS
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("SCHEMAS")
	D SS^BMXADO(.OUT,SIEN,"","B~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
SD	; RETURN THE SCHEMA DEFINITION
	N OUT,%,SIEN
	S SIEN=$$SCHEMA("SCHEMA DEFINITION")
	D SS^BMXADO(.OUT,SIEN,"52,","~~~")
	D DISP(OUT) R %:$G(DTIME,60)
	K ^TMP("BMX ADO",$J)
	Q
	;
FLDIT(PARAM,IENS,MAX,OUT,TOT)	; CUSTOM ITERATOR TO DISPLAY FIELDS
	N SFARR,CNT,DEL,NUM,NAME,DDT,DLEN,DHDR,DRO,DKEY,DNA,X,Y
	D FLIST^BMXADOS(.SFARR,PARAM)
	S CNT=0,DEL="  ["
	F  S CNT=$O(SFARR(CNT)) Q:'CNT  D
	. S X=SFARR(CNT) I '$L(X) Q
	. S NAME=$P(X,DEL)
	. ; F  Q:$E(NAME)'=" "  S NAME=$E(NAME,2,999)
	. I '$L(NAME) Q
	. S NUM=+$P(X,DEL,2) I 'NUM Q
	. S TOT=TOT+1
	. S Y=$$FDEF^BMXADOS(PARAM,NUM) I '$L(Y) Q  ; ""
	. S DDT=$E(Y),DLEN=+$E(Y,2,6),DHDR=$E(Y,7,99)
	. S DRO="NO" S DKEY="NO" S DNA="YES"
	. S ^TMP("BMX ADO",$J,TOT)=NUM_U_NAME_U_DDT_U_DLEN_U_DHDR_U_DRO_U_DKEY_U_DNA_$C(30)
	Q ""
	;
FNIT(PARAM,IENS,MAX,OUT,TOT)	; CUSTOM ITERATOR TO DISPLAY FILE OR SUBFILE NAME GIVEN FILE NUMBER
	N NUM,NAME
	S NUM=+PARAM
	S NAME=""
	Q:'$D(^DD(NUM,0,"NM")) ""
	S NAME=$O(^DD(NUM,0,"NM",0))
	S TOT=TOT+1
	S ^TMP("BMX ADO",$J,TOT)=NUM_U_NAME_$C(30)
	Q ""
	;
SFIT(PARAM,IENS,MAX,OUT,TOT)	; CUSTOM ITERATOR TO DISPLAY SUBFILES
	N SFARR,CNT,DEL,NUM,NAME
	D SC^BMXADOS(.SFARR,PARAM)
	S CNT=0,DEL="  ("
	F  S CNT=$O(SFARR(CNT)) Q:'CNT  D
	. S X=SFARR(CNT) I '$L(X) Q
	. S NAME=$P(X,DEL)
	. ; F  Q:$E(NAME)'=" "  S NAME=$E(NAME,2,999)
	. I '$L(NAME) Q
	. S NUM=+$P(X,DEL,2) I 'NUM Q
	. S TOT=TOT+1
	. S ^TMP("BMX ADO",$J,TOT)=NUM_U_NAME_$C(30)
	Q ""
	; 
SFT(FNAME)	; TRIGGER "YES" TO INDICATE THAT A SUBFILE IS PRESENT WITHIN A FILE
	I '$L($G(FNAME)) Q ""
	N FIEN
	S FIEN=$O(^DIC("B",FNAME,0))
	I 'FIEN Q ""
	I '$O(^DD(FIEN,"SB",0)) Q ""
	Q "+"
	; 
