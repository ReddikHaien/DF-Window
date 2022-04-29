using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Protobuf;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class DfClient
{

    private enum RpcCodes{
        RpcReplyResult = -1,
        RpcReplyFail = -2,
        RpcReplyText = -3,
        RpcRequestQuit = -4,
    }
    private readonly TcpClient client;

    public DfClient(string url, short port){
        client = new TcpClient(url,port);

        Write(CreateHandShake());
        
        var response = Read(12);
        
        var message = Encoding.ASCII.GetString(response);
        if (!message.StartsWith("DFHack!\n")){
            throw new Exception("Failed to Connect To Dwarf Fortress: " + message);
        }
        else{
            Debug.Log("Connected!!");
        }
    }

    public short BindMethod(string method, string input, string output, string plugin = null){
        var request = new Dfproto.CoreBindRequest{
            Method = ByteString.CopyFromUtf8(method),
            InputMsg = ByteString.CopyFromUtf8(input),
            OutputMsg = ByteString.CopyFromUtf8(output),
            Plugin = plugin != null ? ByteString.CopyFromUtf8(plugin) : ByteString.Empty
        };

        SendRequest(0,request);        

        var reply = DecodeMessage<Dfproto.CoreBindReply>(Dfproto.CoreBindReply.Parser);

        return (short) reply.AssignedId;
    }

    public TOut SendMessage<TOut>(short messageId, MessageParser<TOut> parser = null)
        where TOut: class, IMessage<TOut>, new() => SendMessage<Dfproto.EmptyMessage,TOut>(messageId, new Dfproto.EmptyMessage(), parser);

    public TOut SendMessage<TIn, TOut>(short messageId, TIn message, MessageParser<TOut> parser = null)
        where TIn: class, IMessage<TIn>
        where TOut: class, IMessage<TOut>, new()
    {
        SendRequest(messageId,message);
        
        return DecodeMessage<TOut>(parser ?? new MessageParser<TOut>(() => new TOut()));
    }

    private T DecodeMessage<T>(MessageParser<T> parser)
        where T: class, IMessage<T>
    {
        var (code, response) = ReadResponse();

        switch(code){
            case RpcCodes.RpcReplyResult: {
                return parser.ParseFrom(response);
            }
            case RpcCodes.RpcReplyText: {
                Debug.Log("[DFHack]: " + Encoding.ASCII.GetString(response));
                throw new Exception("OHNO");
            }
            default:
            {
                throw new Exception("Received Bad Code " + code);
            }
        }
    }
    //Påtvunget rekompilering
    private byte[] EncodeMessage(short id, IMessage message){
        byte[] payload = message.ToByteArray();
        byte[] res = new byte[8 + payload.Length];
        byte[] idBytes = BitConverter.GetBytes(id);
        byte[] sizeBytes = BitConverter.GetBytes(message.CalculateSize());
        if (!BitConverter.IsLittleEndian){
            Array.Reverse(sizeBytes);
            Array.Reverse(idBytes);
        }
        Array.Copy(idBytes,0,res,0,2);
        res[2] = 0;
        res[3] = 0;
        Array.Copy(sizeBytes,0,res,4,4);
        Array.Copy(payload,0,res,8,payload.Length);

        return res;
    }

    private void SendRequest(short id, IMessage message){
        var byteMessage = EncodeMessage(id,message);
        Write(byteMessage);
    }

    private (RpcCodes,byte[]) ReadResponse(){
        var idBytes = Read(2);
        Read(2);
        var sizeBytes = Read(4);
        if (!BitConverter.IsLittleEndian){
            Array.Reverse(idBytes);
            Array.Reverse(sizeBytes);
        }
        var id = BitConverter.ToInt16(idBytes);
        var size = BitConverter.ToInt32(sizeBytes);

        var payload = Read(size);

        return ((RpcCodes)id,payload);
    }

    private void Write(byte[] data){
        client.GetStream().Write(data,0,data.Length);
    }

    private byte[] Read(int count){
        var bytes = new byte[count];
        int offset = 0;
        int remaining = count;
        while(remaining > 0){
            int amt = client.GetStream().Read(bytes,offset,remaining);
            if (amt <= 0){
                throw new EndOfStreamException($"Ran Out Of Bytes With {remaining} Left to read");
            }
            remaining -= amt;
            offset += amt;
        }
        return bytes;
    }

    private byte[] CreateHandShake(){
        var bytes = Encoding.ASCII.GetBytes("DFHack?\n____");
        var num = BitConverter.GetBytes(1);
        if (!BitConverter.IsLittleEndian){
            Array.Reverse(num);
        }
        bytes[8] = num[0];
        bytes[9] = num[1];
        bytes[10] = num[2];
        bytes[11] = num[3];

        return bytes;
    }

    ~DfClient(){
        SendRequest((short)RpcCodes.RpcRequestQuit, new Dfproto.EmptyMessage());
    }
}
