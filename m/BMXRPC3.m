BMXRPC3	; IHS/OIT/HMW - BMX REMOTE PROCEDURE CALLS ;  ; 8/30/10 2:56pm
	;;2.31;BMX;;Jul 25, 2011
	;Mods by WV/SMH
	;7/26/09 Removed references to ^AUTTSITE, an IHS file in GETFAC*
	;8/30/10 Changed GETFCRS to return a better list of user divisions
	   ; - Checks to see if there are any divisions
	;
VARVAL(RESULT,VARIABLE)	;returns value of passed in variable
	S VARIABLE=$TR(VARIABLE,"~","^")
	S RESULT=VARIABLE ;can do this with the REFERENCE type parameter
	Q
	;See GETV^XWBBRK for how we get the REFERENCE type parameter
	;
USER(RESULT,D)	;
	;
	I '+D S RESULT="" Q
	S RESULT=$P($G(^VA(200,D,0)),"^")
	Q
	;
NTUSER(BMXY,BMXNTUSER)	  ;EP
	;Old code.  Retain for reference
	;Returns NTDomain^NTUserName^RPMSName for user having DUZ=D
	;TODO:  Move ANMC NT USERS file
	;from AZZWNT to BMX namespace and numberspace
	;
	;N BMX,BMXNOD,BMXDOM,BMXNAM,BMXCOL,BMXRNAM
	;S (BMXDOM,BMXNAM,BMXRNAM)=""
	;S U="^"
	;I '+D S RESULT="" Q
	;S BMXRNAM=$G(^VA(200,D,0)),BMXRNAM=$P(BMXRNAM,U)
	;I '$D(^AZZWNT("DUZ",D)) D NTU1 Q
	;S BMX=$O(^AZZWNT("DUZ",D,0))
	;I '+BMX D NTU1 Q
	;I '$D(^AZZWNT(BMX,0)) D NTU1 Q
	;S BMXNOD=^AZZWNT(BMX,0)
	;S BMXDOM=$P(BMXNOD,U,2)
	;S BMXNAM=$P(BMXNOD,U) ;,4)
	;D NTU1
	Q
	;
	;
NTUGETD(BMXY,BMXNTNAME)	;EP
	;Entry point for debugging
	;
	D DEBUG^%Serenji("NTUGET^BMXRPC3(.BMXY,BMXNTNAME)")
	Q
	;
NTUGET(BMXY,BMXNTNAME)	;EP
	;
	;Returns A ENCRYPTED and V ENCRYPTED for NT User BMXNTNAME
	;Called by RPC BMXNetGetCodes
	N BMXI,BMXNTID,BMXNTID,BMXNOD,BMXA,BMXV
	S BMXI=0
	S BMXY="^BMXTMP("_$J_")"
	S X="NTUET^BMXRPC3",@^%ZOSF("TRAP")
	S BMXI=BMXI+1
	I BMXNTNAME="" S ^BMXTMP($J,BMXI)="^" Q
	S BMXNTID=$O(^BMXUSER("B",BMXNTNAME,0))
	I '+BMXNTID S ^BMXTMP($J,BMXI)="^" Q
	S BMXNOD=$G(^BMXUSER(BMXNTID,0))
	S BMXA=$P(BMXNOD,U,2)
	S BMXV=$P(BMXNOD,U,3)
	S ^BMXTMP($J,BMXI)=BMXA_"^"_BMXV_"^"
	Q
	;
WINUGET(BMXWINID)	;EP
	;Returns DUZ for user having Windows Identity BMXWINID
	;Returns 0 if no Windows user found
	;
	N BMXIEN,BMXNOD,BMXDUZ
	I BMXWINID="" Q 0
	S BMXIEN=$O(^BMXUSER("B",BMXWINID,0))
	I '+BMXIEN Q 0
	S BMXNOD=$G(^BMXUSER(BMXIEN,0))
	S BMXDUZ=$P(BMXNOD,U,2)
	Q BMXDUZ
	;
NTUSETD(BMXY,BMXNTNAME)	;EP
	;Entry point for debugging
	;
	D DEBUG^%Serenji("NTUSET^BMXRPC3(.BMXY,BMXNTNAME)")
	Q
	;
NTUSET(BMXY,BMXNTNAME)	;EP
	;Sets NEW PERSON map entry for Windows Identity BMXNTNAME
	;Returns ERRORID 0 if all ok
	;Called by RPC BMXNetSetUser
	;
	;
	N BMXI,BMXNTID,BMXFDA,BMXF,BMXIEN,BMXMSG,BMXAPPTID
	S BMXI=0
	S BMXY="^BMXTMP("_$J_")"
	S X="NTUET^BMXRPC3",@^%ZOSF("TRAP")
	S BMXI=BMXI+1
	; Quit with error if no DUZ exists
	I '+$G(DUZ) D NTUERR(BMXI,500) Q
	; Create entry or file in existing entry in BMX USER
	I $D(^BMXUSER("B",BMXNTNAME)) S BMXF="?1,"
	E  S BMXF="+1,"
	S BMXFDA(90093.1,BMXF,.01)=BMXNTNAME
	S BMXFDA(90093.1,BMXF,.02)=$G(DUZ)
	K BMXIEN,BMXMSG
	D UPDATE^DIE("","BMXFDA","BMXIEN","BMXMSG")
	S BMXAPPTID=+$G(BMXIEN(1))
	S BMXI=BMXI+1
	S ^BMXTMP($J,BMXI)=BMXAPPTID_"^0"
	Q
	;
NTUET	;EP
	;Error trap from REGEVNT
	;
	I '$D(BMXI) N BMXI S BMXI=999
	S BMXI=BMXI+1
	D NTUERR(BMXI,99)
	Q
	;
NTUERR(BMXI,BMXERID)	;Error processing
	S BMXI=BMXI+1
	S ^BMXTMP($J,BMXI)="^"_BMXERID
	Q
	;
	;
NTU1	;S BMXCOL="T00030NT_DOMAIN^T00030NT_USERNAME^T00030RPMS_USERNAME"_$C(30)
	;S RESULT=BMXCOL_BMXDOM_U_BMXNAM_U_BMXRNAM_$C(30)_$C(31)
	Q
	;
GETFC(BMXFACS,DUZ)	;Gets all facilities for a user
	; Input DUZ - user IEN from the NEW PERSON FILE
	; Output - Number of facilities;facility1 name&facility1 IEN;...facilityN&facilityN IEN
	N BMXFN,BMXN
	S BMXFN=0,BMXFACS=""
	F BMXN=1:1 S BMXFN=$O(^VA(200,DUZ,2,BMXFN)) Q:BMXFN=""  D
	. S:BMXN>1 BMXFACS=BMXFACS_";" S BMXFACS=BMXFACS_$P(^DIC(4,BMXFN,0),U,1)_"&"_BMXFN
	;//smh I BMXN=1 S BMXFN=$P(^AUTTSITE(1,0),U,1) D 
	;//smh . S BMXFACS=BMXFACS_$P(^DIC(4,BMXFN,0),U,1)_"&"_BMXFN
	S BMXFACS=BMXN-(BMXN>1)_";"_BMXFACS
	Q
	;
GETFCRS(BMXY,BMXDUZ)	;Gets all facilities for a user - returns RECORDSET
	;/mods by //smh for WV
	N $ET S $ET="G ERFC^BMXRPC3"
	   N BMXFN    ; Facility Number
	S BMXDUZ=$TR(BMXDUZ,$C(13)) ; Strip CR,LF,tab
	S BMXDUZ=$TR(BMXDUZ,$C(10))
	S BMXDUZ=$TR(BMXDUZ,$C(9))
	S BMXY="T00030FACILITY_NAME^T00030FACILITY_IEN^T00002DEFAULT"_$C(30)
	S BMXFN=0
	F  S BMXFN=$O(^VA(200,BMXDUZ,2,BMXFN)) Q:'BMXFN  D
	. ; DD for ^VA(200,DUZ,2,DUZ(2)) is DUZ(2)^default. DUZ(2) is dinummed.
	   . S BMXY=BMXY_$P(^DIC(4,BMXFN,0),U,1)_U_^VA(200,BMXDUZ,2,BMXFN,0)_$C(30)
	   ; Crazy line: if we have no results, then use kernel's DUZ(2) set
	   ; during sign-on
	   I $L(BMXY,$C(30))<3 S BMXY=BMXY_$P(^DIC(4,DUZ(2),0),U,1)_U_DUZ(2)_$C(30)
	S BMXY=BMXY_$C(31)
	Q
	;
SETFCRS(BMXY,BMXFAC)	     ;
	;
	;Sets DUZ(2) to value in BMXFAC
	;Fails if BMXFAC is not one of the current user's divisions
	;Returns Recordset
	;
	S X="ERFC^BMXRPC3",@^%ZOSF("TRAP")
	S BMXY="T00030DUZ^T00030FACILITY_IEN^T00030FACILITY_NAME"_$C(30)
	N BMXSUB,BMXFACN
	I '+DUZ S BMXY=BMXY_0_"^"_0_"^"_0_$C(30)_$C(31) Q
	I '+BMXFAC S BMXY=BMXY_DUZ_"^"_0_"^"_0_$C(30)_$C(31) Q
	; //SMH Line below is incorrect. Facility valid if not in user profile
	   ; if it is default kernel facility
	   ; I '$D(^VA(200,DUZ,2,+BMXFAC)) S BMXY=BMXY_DUZ_"^"_0_"^"_0_$C(30)_$C(31) Q
	S DUZ(2)=BMXFAC ;IHS/OIT/HMW SAC Exemption Applied For
	S BMXFACN=$G(^DIC(4,+DUZ(2),0))
	S BMXFACN=$P(BMXFACN,"^")
	S BMXSUB="^VA(200,"_DUZ_",2,"
	S ^DISV(DUZ,BMXSUB)=BMXFAC
	S BMXY=BMXY_DUZ_"^"_BMXFAC_"^"_BMXFACN_$C(30)_$C(31)
	Q
	;
ERFC	;
	D ^%ZTER
	S BMXY=$G(BMXY)_0_"^"_0_$C(30)_$C(31) Q
	Q
	;
SETFC(BMXY,BMXFAC)	;
	;Sets DUZ(2) to value in BMXFAC
	;Fails if BMXFAC is not one of the current user's divisions
	;Returns 1 if successful, 0 if failed
	;
	S BMXY=0
	N BMXSUB
	I '+DUZ S BMXY=0 Q
	I '+BMXFAC S BMXY=0 Q
	I '$D(^VA(200,DUZ,2,+BMXFAC)) S BMXY=0 Q
	S DUZ(2)=BMXFAC ;IHS/OIT/HMW SAC Exemption Applied For
	S BMXSUB="^VA(200,"_DUZ_",2,"
	S ^DISV(DUZ,BMXSUB)=BMXFAC
	S BMXY=1
	Q
	;
APSEC(BMXY,BMXKEY)	      ;EP
	;Return IHSCD_SUCCEEDED (-1) if user has key BMXKEY
	;OR if user has key XUPROGMODE
	;Otherwise, returns IHSCD_FAILED (0)
	N BMXIEN,BMXPROG,BMXPKEY
	I '$G(DUZ) S BMXY=0 Q
	I BMXKEY="" S BMXY=0 Q
	;
	;Test for programmer mode key
	S BMXPROG=0
	I $D(^DIC(19.1,"B","XUPROGMODE")) D
	. S BMXPKEY=$O(^DIC(19.1,"B","XUPROGMODE",0))
	. I '+BMXPKEY Q
	. I '$D(^VA(200,DUZ,51,BMXPKEY,0)) Q
	. S BMXPROG=1
	I BMXPROG S BMXY=-1 Q
	;
	I '$D(^DIC(19.1,"B",BMXKEY)) S BMXY=0 Q
	S BMXIEN=$O(^DIC(19.1,"B",BMXKEY,0))
	I '+BMXIEN S BMXY=0 Q
	I '$D(^VA(200,DUZ,51,BMXIEN,0)) S BMXY=0 Q
	S BMXY=-1
	Q
	;
SIGCHK(BMXY,BMXSIG)	        ;EP
	;Checks BMXSIG against hashed value in NEW PERSON
	;Return IHSCD_SUCCEEDED (-1) if BMXSIG matches
	;Otherwise, returns IHSCD_FAILED (0)
	N X
	S BMXY=0
	I '$G(DUZ) Q
	I '$D(^VA(200,DUZ,20)) Q  ;TODO What if no signature?
	S BMXHSH=$P(^VA(200,DUZ,20),U,4)
	S X=$G(BMXSIG)
	D HASH^XUSHSHP
	I X=BMXHSH S BMXY=-1
	Q
