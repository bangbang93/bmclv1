using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace bmcl.serverlist
{
    public class serverlist
    {
        public int count = 0;
        private byte[] header = new byte[0x13];
        private ArrayList list = new ArrayList(5);
        public serverinfo[] info;
        public serverlist()
        {
            FileStream server = new FileStream(@".minecraft\servers.dat", FileMode.Open);
            server.Read(header, 0, header.Length);
            if (server.Length-server.Position < 5)
            {
                server.Close();
                info = (serverinfo[])list.ToArray(typeof(serverinfo));
                return;
            }
            while (server.Position != server.Length)
            {
                byte[] HideAddress = new byte[13];
                server.Read(HideAddress, 0, HideAddress.Length);
                bool IsHide = (server.ReadByte())==1?true:false;
                byte[] name = new byte[9];
                server.Read(name, 0, name.Length);
                ArrayList NameString=new ArrayList(20);
                byte ch = (byte)server.ReadByte();
                while (ch != 0)
                {
                    NameString.Add(ch);
                    ch = (byte)server.ReadByte();
                }
                string Name = Encoding.UTF8.GetString((byte[])NameString.ToArray(typeof(byte)), 0, NameString.Count - 1);
                byte[] address = new byte[5];
                server.Read(address, 0, address.Length);
                ArrayList AddRess = new ArrayList(20);
                ch = (byte)server.ReadByte();
                while (ch != 0)
                {
                    AddRess.Add(ch);
                    ch = (byte)server.ReadByte();
                }
                string Address = Encoding.UTF8.GetString((byte[])AddRess.ToArray(typeof(byte)), 0, AddRess.Count);
                list.Add(new serverinfo(Name, IsHide, Address));
                if (server.ReadByte() == 0)
                    break;
            }
            info = (serverinfo[])list.ToArray(typeof(serverinfo));
            server.Close();
        }
        public void Add(string name,string ip,bool ishide)
        {
            
            serverinfo newserver = new serverinfo(name, ishide, ip);
            list.Add(newserver);
            info = (serverinfo[])list.ToArray(typeof(serverinfo));
        }
        public void Write()
        {
            FileStream serverdat = new FileStream(@".minecraft\servers.dat", FileMode.Create);
            serverdat.Write(Convert.FromBase64String(resource.ServerDat.Header), 0, Convert.FromBase64String(resource.ServerDat.Header).Length);
            int p = 0;
            serverdat.WriteByte(0);
            serverdat.WriteByte((byte)list.Count);
            serverdat.WriteByte(1);
            foreach (serverinfo aserver in list)
            {
                p++;
                serverdat.Write(Convert.FromBase64String(resource.ServerDat.HideAddress), 0, Convert.FromBase64String(resource.ServerDat.HideAddress).Length);
                if (aserver.IsHide)
                    serverdat.WriteByte(1);
                else
                    serverdat.WriteByte(0);
                serverdat.WriteByte(8);
                serverdat.Write(Convert.FromBase64String(resource.ServerDat.name), 0, Convert.FromBase64String(resource.ServerDat.name).Length);
                serverdat.WriteByte((byte)Encoding.UTF8.GetBytes(aserver.Name).Length);
                serverdat.Write(Encoding.UTF8.GetBytes(aserver.Name),0,Encoding.UTF8.GetBytes(aserver.Name).Length);
                serverdat.Write(new byte[] { 8, 0}, 0, 2);
                serverdat.Write(Convert.FromBase64String(resource.ServerDat.address),0,Convert.FromBase64String(resource.ServerDat.address).Length);
                serverdat.WriteByte((byte)Encoding.UTF8.GetBytes(aserver.Address).Length);
                serverdat.Write(Encoding.UTF8.GetBytes(aserver.Address),0,Encoding.UTF8.GetBytes(aserver.Address).Length);
                serverdat.WriteByte(0);
                if (p!=list.Count)
                    serverdat.WriteByte(1);
                else
                    serverdat.WriteByte(0);
            }
            serverdat.Close();
        }

        public void Delete(int num)
        {
            list.RemoveAt(num);
            info = (serverinfo[])list.ToArray(typeof(serverinfo));
        }

        public void Edit(int num, string Name, string Address, bool IsHide)
        {
            serverinfo aserver = new serverinfo(Name, IsHide, Address);
            list[num] = aserver;
            info = (serverinfo[])list.ToArray(typeof(serverinfo));
        }
    }
}
