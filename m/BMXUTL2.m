BMXUTL2	; IHS/OIT/HMW - UTIL: PATIENT INFO ;
	;;2.31;BMX;;Jul 25, 2011
	;;Stolen from:* MICHAEL REMILLARD, DDS * ALASKA NATIVE MEDICAL CENTER *
	;;  UTILITY: PATIENT FUNCTIONS: CONTRAS, INPATIENT, HIDOSE.
	;
NEXTAPPT(BMXDFN)	;EP
	;---> Return patient's next appointment from Scheduling Package.
	;---> Parameters:
	;     1 - BMXDFN  (req) Patient's IEN (BMXDFN).
	;
	Q:'$G(BMXDFN) ""
	Q:'$D(^DPT(BMXDFN)) ""
	;
	N BMXAPPT,BMXDT,BMXYES
	S BMXDT=DT+.2400,BMXYES=0
	F  S BMXDT=$O(^DPT(BMXDFN,"S",BMXDT)) Q:'BMXDT!(BMXYES)  D
	.N BMXDATA,BMXOI,X
	.S BMXDATA=$G(^DPT(BMXDFN,"S",BMXDT,0))
	.Q:BMXDATA=""
	.;
	.;---> Quit if appointment is cancelled.
	.Q:$P(BMXDATA,U,2)["C"
	.;
	.S X=0 F  S X=$O(^SC(+BMXDATA,"S",BMXDT,1,X)) Q:'X  D
	..Q:+$G(^SC(+BMXDATA,"S",BMXDT,1,X,0))'=BMXDFN
	..S BMXYES=BMXDT_U_+BMXDATA
	;
	Q:'BMXYES ""
	;
	S BMXAPPT=$$FMTE^XLFDT(+BMXYES,"1P")_" with "
	S BMXAPPT=BMXAPPT_$P($G(^SC($P(BMXYES,U,2),0)),U)
	Q BMXAPPT
