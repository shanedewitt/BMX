BMXRPC4	; IHS/OIT/HMW - BMX REMOTE PROCEDURE CALLS ;
	;;2.1;BMX;;Jul 26, 2009
	;
PTINFORS(BMXY,BMXIEN)	       ;EP Patient Info Recordset
	;
	N BMXDPT,BMXZ,BMXDLIM,BMXXX,BMXRET,BMXAGE,BMXNEXT,BMXSEX,BMXERR,BMXHRN
	S BMXDLIM="^",BMXERR=""
	S BMXRET="T00030NAME^T00030HRN^T00030SSN^D00030DOB^T00030IEN^T00020AGE^T00080NEXT_APPT^T00010SEX"_$C(30)
	I '$D(DUZ(2)) S BMXY=BMXRET_$C(31)_"No DUZ2" Q
	I +$G(DUZ) D
	. S ^DISV(DUZ,"^AUPNPAT(")=BMXIEN
	. S ^DISV(DUZ,"^DPT(")=BMXIEN
	I '$D(^DPT(BMXIEN)) S BMXY=BMXRET_$C(31)_"No such patient" Q
	S BMXDPT=$G(^DPT(BMXIEN,0))
	S BMXZ=$P(BMXDPT,U) ;NAME
	;S $P(BMXZ,BMXDLIM,2)=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	S BMXHRN=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	;I BMXHRN="" Q  ;NO CHART AT THIS DUZ2
	I $P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,3) S BMXHRN=BMXHRN_"(*)"
	S $P(BMXZ,BMXDLIM,2)=BMXHRN
	;
	S $P(BMXZ,BMXDLIM,3)=$P(BMXDPT,U,9) ;SSN
	S Y=$P(BMXDPT,U,3) X ^DD("DD")
	S $P(BMXZ,BMXDLIM,4)=Y ;DOB
	S $P(BMXZ,BMXDLIM,5)=BMXIEN
	S BMXAGE=$$AGEF^BMXUTL1(BMXIEN)
	S $P(BMXZ,BMXDLIM,6)=BMXAGE
	S BMXNEXT=$$NEXTAPPT^BMXUTL2(BMXIEN)
	S $P(BMXZ,BMXDLIM,7)=BMXNEXT
	S BMXSEX=$$SEXW^BMXUTL1(BMXIEN)
	S $P(BMXZ,BMXDLIM,8)=BMXSEX
	S BMXRET=BMXRET_BMXZ
	S BMXY=BMXRET_$C(30)_$C(31)_BMXERR
	Q
	;
PTLOOKRS(BMXY,BMXP,BMXC)	 ;EP Patient Lookup
	;
	;Find up to BMXC patients matching BMXP*
	;Supports DOB Lookup, SSN Lookup
	;
	;S ^HW("PTLOOK","INPUT")=BMXP
	;S ^HW("PTLOOK","DUZ2")=$G(DUZ(2))
	S BMXP=$TR(BMXP,$C(13),"")
	S BMXP=$TR(BMXP,$C(10),"")
	S BMXP=$TR(BMXP,$C(9),"")
	S:BMXC="" BMXC=10
	N BMXHRN,BMXZ,BMXDLIM,BMXRET
	S BMXDLIM="^"
	S BMXRET="T00030NAME^T00030HRN^T00030SSN^D00030DOB^T00030IEN"_$C(30)
	I '+$G(DUZ) S BMXY=BMXRET_$C(31) Q
	I '$D(DUZ(2)) S BMXY=BMXRET_$C(31) Q
DOB	;DOB Lookup
	I +DUZ(2),((BMXP?1.2N1"/"1.2N1"/"1.4N)!(BMXP?1.2N1" "1.2N1" "1.4N)!(BMXP?1.2N1"-"1.2N1"-"1.4N)) D  S BMXY=BMXRET_$C(31) Q
	. S X=BMXP S %DT="P" D ^%DT S BMXP=Y Q:'+Y
	. Q:'$D(^DPT("ADOB",BMXP))
	. S BMXIEN=0,BMXXX=1 F  S BMXIEN=$O(^DPT("ADOB",BMXP,BMXIEN)) Q:'+BMXIEN  D
	. . Q:'$D(^DPT(BMXIEN,0))
	. . S BMXDPT=$G(^DPT(BMXIEN,0))
	. . S BMXZ=$P(BMXDPT,U) ;NAME
	. . ;S $P(BMXZ,BMXDLIM,2)=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	. . S BMXHRN=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	. . I BMXHRN="" Q  ;NO CHART AT THIS DUZ2
	. . I $P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,3) S BMXHRN=BMXHRN_"(*)"
	. . S $P(BMXZ,BMXDLIM,2)=BMXHRN
	. . ;
	. . S $P(BMXZ,BMXDLIM,3)=$P(BMXDPT,U,9) ;SSN
	. . S Y=$P(BMXDPT,U,3) X ^DD("DD")
	. . S $P(BMXZ,BMXDLIM,4)=Y ;DOB
	. . S $P(BMXZ,BMXDLIM,5)=BMXIEN
	. . S BMXXX=BMXXX+1
	. . ;S $P(BMXRET,$C(30),BMXXX)=BMXZ
	. . S BMXRET=BMXRET_BMXZ_$C(30)
	. . Q
	. Q
	;
	;Chart# Lookup
	I +DUZ(2),BMXP]"",$D(^AUPNPAT("D",BMXP)) D  S BMXY=BMXRET_$C(30)_$C(31) Q
	. S BMXIEN=0 F  S BMXIEN=$O(^AUPNPAT("D",BMXP,BMXIEN)) Q:'+BMXIEN  I $D(^AUPNPAT("D",BMXP,BMXIEN,DUZ(2))) D  Q
	. . Q:'$D(^DPT(BMXIEN,0))
	. . S BMXDPT=$G(^DPT(BMXIEN,0))
	. . S BMXZ=$P(BMXDPT,U) ;NAME
	. . ;S $P(BMXZ,BMXDLIM,2)=BMXP ;CHART
	. . S BMXHRN=BMXP ;CHART
	. . I $D(^AUPNPAT(BMXIEN,41,DUZ(2),0)),$P(^(0),U,3) S BMXHRN=BMXHRN_"(*)"
	. . S $P(BMXZ,BMXDLIM,2)=BMXHRN
	. . S $P(BMXZ,BMXDLIM,3)=$P(BMXDPT,U,9) ;SSN
	. . S Y=$P(BMXDPT,U,3) X ^DD("DD")
	. . S $P(BMXZ,BMXDLIM,4)=Y ;DOB
	. . S $P(BMXZ,BMXDLIM,5)=BMXIEN
	. . S $P(BMXRET,$C(30),2)=BMXZ
	. . Q
	. Q
	;
	;SSN Lookup
	I (BMXP?9N)!(BMXP?3N1"-"2N1"-"4N),$D(^DPT("SSN",BMXP)) D  S BMXY=BMXRET_$C(30)_$C(31) Q
	. S BMXIEN=0 F  S BMXIEN=$O(^DPT("SSN",BMXP,BMXIEN)) Q:'+BMXIEN  D  Q
	. . Q:'$D(^DPT(BMXIEN,0))
	. . S BMXDPT=$G(^DPT(BMXIEN,0))
	. . S BMXZ=$P(BMXDPT,U) ;NAME
	. . S BMXHRN=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	. . I BMXHRN="" Q  ;NO CHART AT THIS DUZ2
	. . I $P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,3) S BMXHRN=BMXHRN_"(*)"
	. . S $P(BMXZ,BMXDLIM,2)=BMXHRN
	. . S $P(BMXZ,BMXDLIM,3)=$P(BMXDPT,U,9) ;SSN
	. . S Y=$P(BMXDPT,U,3) X ^DD("DD")
	. . S $P(BMXZ,BMXDLIM,4)=Y ;DOB
	. . S $P(BMXZ,BMXDLIM,5)=BMXIEN
	. . S $P(BMXRET,$C(30),2)=BMXZ
	. . Q
	. Q
	;
	S BMXFILE=9000001
	S BMXIENS=""
	S BMXFIELDS=".01"
	S BMXFLAGS="M"
	S BMXVALUE=BMXP
	S BMXNUMBER=BMXC
	S BMXINDEXES=""
	S BMXSCREEN=$S(+DUZ(2):"I $D(^AUPNPAT(Y,41,DUZ(2),0))",1:"")
	;I BMXSCREEN]"" S DIC("S")=BMXSCREEN
	;S BMXSCREEN="I 0"
	S BMXIDEN=""
	S BMXTARG="BMXRSLT"
	S BMXMSG=""
	D FIND^DIC(BMXFILE,BMXIENS,BMXFIELDS,BMXFLAGS,BMXVALUE,BMXNUMBER,BMXINDEXES,BMXSCREEN,BMXIDEN,BMXTARG,BMXMSG)
	;S BMXRET=""
	;B
	I '+$G(BMXRSLT("DILIST",0)) S BMXY=BMXRET_$C(31) Q
	F BMXX=1:1:$P(BMXRSLT("DILIST",0),U) D
	. ;B
	. S BMXIEN=BMXRSLT("DILIST",2,BMXX)
	. S BMXZ=BMXRSLT("DILIST","ID",BMXX,.01) ;NAME
	. ;S $P(BMXZ,BMXDLIM,2)=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	. S BMXHRN=$P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,2) ;CHART
	. I BMXHRN="" Q  ;NO CHART AT THIS DUZ2
	. I $P($G(^AUPNPAT(BMXIEN,41,DUZ(2),0)),U,3) S BMXHRN=BMXHRN_"(*)"
	. S $P(BMXZ,BMXDLIM,2)=BMXHRN
	. S BMXDPT=$G(^DPT(BMXIEN,0))
	. S $P(BMXZ,BMXDLIM,3)=$P(BMXDPT,U,9) ;SSN
	. S Y=$P(BMXDPT,U,3) X ^DD("DD")
	. S $P(BMXZ,BMXDLIM,4)=Y ;DOB
	. S $P(BMXZ,BMXDLIM,5)=BMXIEN
	. S $P(BMXRET,$C(30),BMXX+1)=BMXZ
	. Q
	;K BMXRSLT
	S BMXY=BMXRET_$C(30)_$C(31)
	Q
ZZZ	;
