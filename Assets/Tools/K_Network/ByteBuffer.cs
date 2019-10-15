using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class ByteBuffer : IDisposable
        {
            public List<byte> Buff;
            private byte[] readBuff;
            private int readPos;
            private bool buffUpdated = false;

            #region Class
            public ByteBuffer()
            {
                Buff = new List<byte>();
                readPos = 0;
            }

            //Getter
            public int ReadPos
            {
                get
                {
                    return readPos;
                }
            }
            public byte[] ToArray()
            {
                return Buff.ToArray();
            }
            public int Count
            {
                get
                {
                    return Buff.Count;
                }
            }
            public int Lenght
            {
                get
                {
                    return Count - readPos;
                }
            }
            public void Clear()
            {
                Buff.Clear();
                readPos = 0;
            }
            #endregion

            #region Writter
            public void WriteByte(byte input)
            {
                Buff.Add(input);
                buffUpdated = true;
            }
            public void WriteBytes(byte[] inputs)
            {
                Buff.AddRange(inputs);
                buffUpdated = true;
            }
            public void WriteShort(short input)
            {
                Buff.AddRange(BitConverter.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteInteger(int input)
            {
                Buff.AddRange(BitConverter.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteLong(long input)
            {
                Buff.AddRange(BitConverter.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteFloat(float input)
            {
                Buff.AddRange(BitConverter.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteBool(bool input)
            {
                Buff.AddRange(BitConverter.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteString(string input)
            {
                Buff.AddRange(BitConverter.GetBytes(input.Length));
                Buff.AddRange(Encoding.ASCII.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteGuid(Guid input)
            {
                Buff.AddRange(BitConverter.GetBytes(36));//GUID_LENGHT
                //Buff.AddRange(input.get Encoding.ASCII.GetBytes(input));
                buffUpdated = true;
            }
            public void WriteVector3(Vector3 input)
            {
                Buff.AddRange(BitConverter.GetBytes(input.x));
                Buff.AddRange(BitConverter.GetBytes(input.y));
                Buff.AddRange(BitConverter.GetBytes(input.z));
                buffUpdated = true;
            }
            #endregion

            #region Reader
            public byte ReadByte(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    byte value = readBuff[readPos];
                    if (peek && Buff.Count > readPos)
                    {
                        readPos++;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range");
                }
            }
            public byte[] ReadBytes(int lenght, bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    byte[] value = Buff.GetRange(readPos, Lenght).ToArray();
                    if (peek)
                    {
                        readPos += lenght;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range");
                }
            }

            #region ReaderSimplify
            /*
            public short ReadShort(bool peek = true)
            {
                byte[] value = ReadBytes(2,peek);
                return BitConverter.ToInt16(value, readPos);
            }
            public int ReadInteger(bool peek = true)
            {
                byte[] value = ReadBytes(4,peek);
                return BitConverter.ToInt32(value, readPos);
            }
            public long ReadLong(bool peek = true)
            {
                byte[] value = ReadBytes(8,peek);
                return BitConverter.ToInt64(value, readPos);
            }
            public float ReadFloat(bool peek = true)
            {
                byte[] value = ReadBytes(4,peek);
                return BitConverter.ToSingle(value, readPos);
            }
            public bool ReadBool(bool peek = true)
            {
                byte[] value = ReadBytes(1,peek);
                return BitConverter.ToBoolean(value, readPos);
            }
            public string ReadString(bool peek = true)
            {
                int lenght = ReadInteger(true);

                byte[] value = ReadBytes(lenght,peek);
                return Encoding.ASCII.GetString(value);
            }
            */
            #endregion

            #region ReaderBase
            public short ReadShort(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    short value = BitConverter.ToInt16(readBuff, readPos);
                    if (peek && Buff.Count > readPos)
                    {
                        readPos += 2;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range : SHORT");
                }
            }
            public int ReadInteger(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    int value = BitConverter.ToInt32(readBuff, readPos);
                    if (peek && Buff.Count > readPos)
                    {
                        readPos += 4;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range : INT");
                }
            }
            public long ReadLong(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    long value = BitConverter.ToInt64(readBuff, readPos);
                    if (peek && Buff.Count > readPos)
                    {
                        readPos += 8;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range : LONG");
                }
            }
            public float ReadFloat(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    float value = BitConverter.ToSingle(readBuff, readPos);
                    if (peek && Buff.Count > readPos)
                    {
                        readPos += 4;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range : FLOAT");
                }
            }
            public bool ReadBool(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    if (buffUpdated)
                    {
                        readBuff = Buff.ToArray();
                        buffUpdated = false;
                    }

                    bool value = BitConverter.ToBoolean(readBuff, readPos);
                    if (peek && Buff.Count > readPos)
                    {
                        readPos += 1;
                    }
                    return value;
                }
                else
                {
                    throw new Exception("Buffer out of range : BOOL");
                }
            }
            public string ReadString(bool peek = true)
            {
                int lenght = ReadInteger(true);
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                string value = Encoding.ASCII.GetString(readBuff, readPos, lenght);
                if (peek && Buff.Count > readPos)
                {
                    if (value.Length > 0)
                        readPos += lenght;
                }
                return value;
            }

            public Guid ReadGuid(bool peek = true)
            {
                int lenght = ReadInteger(true);
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                string value = Encoding.ASCII.GetString(readBuff, readPos, lenght);
                if (peek && Buff.Count > readPos)
                {
                    if (value.Length > 0)
                        readPos += lenght;
                }
                return value;
            }

            public Vector3 ReadVector3(bool peek = true)
            {
                if (Buff.Count > readPos)
                {
                    Vector3 output = Vector3.zero;
                    output.x = ReadFloat(true);
                    output.y = ReadFloat(true);
                    output.z = ReadFloat(true);

                    //if (peek && Buff.Count > readPos)
                    //{
                    //    readPos += (4 * 3);//float:4 * x,y,z
                    //}
                    return output;
                }
                else
                {
                    throw new Exception("Buffer out of range : BOOL");
                }
            }

            #endregion

            #endregion

            private bool disposedValue = false;
            public virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        Buff.Clear();
                        readPos = 0;
                    }
                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }
    }
}
