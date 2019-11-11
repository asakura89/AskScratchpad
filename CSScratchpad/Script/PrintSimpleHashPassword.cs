using System;
using System.Security.Cryptography;
using System.Text;
using Scratch;

namespace CSScratchpad.Script {
    public class PrintSimpleHashPassword : Common, IRunnable {
        public void Run() {
            Dbg(HashPassword("I_don't-want-this moment. To_ever-end."));
            Dbg(HashPassword("halo galo halio hasga."));
        }

        Decimal HashPassword(String passwordString) {
            Byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(passwordString));
            Int32 distributedHash = 0;
            for (Int32 i = 0; i < 4; i++) {
                Int32 distributedByte = 0;
                for (Int32 j = 0; j < 4; j++) {
                    Byte byteToDistribute = hash[(i * 4) + j];
                    distributedByte = (distributedByte + byteToDistribute) % 0xff;
                }
                distributedHash += distributedByte * ((Int32) Math.Pow(256.0, (Double) i));
            }

            return distributedHash;
        }
    }
}
