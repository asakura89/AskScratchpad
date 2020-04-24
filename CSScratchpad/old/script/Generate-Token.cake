#l "Common.cake"

using System;
using System.Text;

static readonly Int32 MaxChar = 8;

void Script() {
    String[] alphaNumeric = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,0,1,2,3,4,5,6,7,8,9".Split(',');
    var tokenCharList = new String[MaxChar];
    var rand = new Random();
    var strBuilder = new StringBuilder();
    for (Int32 idx = 0; idx < MaxChar; idx++)
        strBuilder.Append(alphaNumeric[rand.Next(0, alphaNumeric.Length -1)]);

    Console.WriteLine("Generated Token: {0}", strBuilder.ToString());
}
