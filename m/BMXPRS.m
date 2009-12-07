BMXPRS	; IHS/OIT/HMW - BMX WINDOWS UTILS ;
	;;2.1;BMX;;Jul 26, 2009
	;
	;
PARSE(X)	;EP-Parse SQL Statement into array
	;Input SQL statement as X
	;Returns BMXTK() array
	;Errors returned in BMXERR
	;
	D PRE
	Q:$D(BMXERR)
	D POST
	Q
	;
POST2	;EP - Remove commas from BMXTK
	N J,K
	S J=0 F  S J=$O(BMXTK(J)) Q:'+J  D
	. S K=$O(BMXTK(J))
	. I +K,","=$G(BMXTK(K)) D
	. . K BMXTK(K)
	. . D PACK(J)
	. . Q
	. Q
	Q
	;
POST	;
	;Combine multi-character operators
	N J
	S J=0 F  S J=$O(BMXTK(J)) Q:'+J  D
	. I ">"=BMXTK(J) D  Q
	. . I "="[$G(BMXTK(J+1)) D  Q
	. . . S BMXTK(J)=BMXTK(J)_"="
	. . . K BMXTK(J+1)
	. . . D PACK(J)
	. . I "<"[$G(BMXTK(J+1)) D  Q
	. . . S BMXTK(J)="<"_BMXTK(J)
	. . . K BMXTK(J+1)
	. . . D PACK(J)
	. I "<"=BMXTK(J) D  Q
	. . I "=>"[$G(BMXTK(J+1)) D
	. . . S BMXTK(J)=BMXTK(J)_BMXTK(J+1)
	. . . K BMXTK(J+1)
	. . . D PACK(J)
	. I "="=BMXTK(J) D  Q
	. . I "<>"[$G(BMXTK(J+1)) D
	. . . S BMXTK(J)=BMXTK(J+1)_BMXTK(J)
	. . . K BMXTK(J+1)
	. . . D PACK(J)
	Q
	;
PACK(J)	;
	F  S J=$O(BMXTK(J)) Q:'+J  D
	. S BMXTK(J-1)=BMXTK(J)
	. K BMXTK(J)
	Q
	;
PRE	N P,T,Q,Q1,A,B S (P,T,Q)=0,BMXTK="",A=0
START	S A=A+1
	S B=$E(X,A)
	I B="" G B5
	I 'Q G QUOTE
	I B=$C(39) G QUOTE
	S BMXTK=BMXTK_B G START
QUOTE	I B'=$C(39) G SPACE
	I Q G QUOTE2
	;S Q=1,BMXTK=B G START
	S Q=1,BMXTK=BMXTK_B G START
QUOTE2	S Q1=B,A=A+1,B=$E(X,A)
	I B']"" G QUOTE3
	I B'=$C(39) G QUOTE3
	S BMXTK=BMXTK_Q1_B G START
QUOTE3	S A=A-1,B=Q1,BMXTK=BMXTK_B,Q=0 G START
SPACE	I B'=" " G OP
	I BMXTK]"" S T=T+1,BMXTK(T)=BMXTK,BMXTK=""
	G START
OP	I "=><"'[B G OPAREN
	I BMXTK]"" S T=T+1,BMXTK(T)=BMXTK,BMXTK=""
	S T=T+1,BMXTK(T)=B,BMXTK=""
	G START
OPAREN	I B'="(" G CPAREN
	S P=P+1
	I BMXTK]"" S T=T+1,BMXTK(T)=BMXTK,BMXTK=""
	S T=T+1,BMXTK(T)=B G START
CPAREN	I B'=")" G B2
	I P G B1
	G B0
	;
B0	S BMXERR="SQL SYNTAX ERROR" D ERROR G B5
B1	S P=P-1
	I BMXTK]"" S T=T+1,BMXTK(T)=BMXTK,BMXTK=""
	S T=T+1,BMXTK(T)=B G START
B2	I B'="," G B3
	S T=T+1,BMXTK(T)=BMXTK,T=T+1,BMXTK(T)=",",BMXTK="" G START
B3	S BMXTK=BMXTK_B
B4	G START
B5	I BMXTK]"" S T=T+1,BMXTK(T)=BMXTK
	I $D(BMXERR) G B6
	I P S BMXERR="SQL SYNTAX ERROR: MATCHING PARENTHESIS NOT FOUND" D ERROR
	E  I Q S BMXERR="SQL SYNTAX ERROR: MATCHING QUOTE NOT FOUND" D ERROR
	I P>0 G START
B6	Q
	;
ERROR	;W !,"ERROR=",BMXERR,! Q  
	Q
