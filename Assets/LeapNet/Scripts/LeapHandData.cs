using System;
using System.Collections.Generic;
using Leap;
using System.IO;
using UnityEngine;

public class LeapHandData
{
    public static byte[] Serialize(Hand hand)
    {
        if (hand == null)
            return null;

        NetHand nethand = new NetHand(hand);
        return Serialize(nethand);
    }

    public static byte[] Serialize(NetHand hand)
    {
        if (hand == null)
            return null;

        byte[] data = null;
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                NetHand.Write(writer, hand);

                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }
        return data;
    }

    // Convert a byte array to an Object
    public static NetHand Deserialize(byte[] arr)
    {
        if (arr == null)
        {
            return null;
        }
        using (MemoryStream stream = new MemoryStream(arr))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return NetHand.Read(reader);
            }
        }
    }
}

public class NetHand
{
    public List<NetFinger> Fingers;
    public bool IsLeft;
    public NetVector PalmPosition;
    public NetVector XBasis;

    public static void Write(BinaryWriter w, NetHand h)
    {
        if (h.Fingers == null || h.Fingers.Count == 0)
        {
            w.Write(0);
        }
        else
        {
            w.Write(h.Fingers.Count);
            foreach (NetFinger f in h.Fingers)
            {
                NetFinger.Write(w, f);
            }
        }
        w.Write(h.IsLeft);
        NetVector.Write(w, h.PalmPosition);
        NetVector.Write(w, h.XBasis);
    }

    public static NetHand Read(BinaryReader r)
    {
        NetHand h = new NetHand();
        int count = r.ReadInt32();
        h.Fingers = new List<NetFinger>();
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                NetFinger f = NetFinger.Read(r);
                h.Fingers.Add(f);
            }
        }
        h.IsLeft = r.ReadBoolean();
        h.PalmPosition = NetVector.Read(r);
        h.XBasis = NetVector.Read(r);

        return h;
    }

    public NetHand() { }

    public NetHand(Hand h)
    {
        Fingers = new List<NetFinger>();
        foreach (Finger f in h.Fingers)
        {
            Fingers.Add(new NetFinger(f));
        }
        IsLeft = h.IsLeft;
        PalmPosition = new NetVector(h.PalmPosition);
        XBasis = new NetVector(h.Basis.xBasis);
    }
}

public class NetVector
{
    public float x;
    public float y;
    public float z;

    public static void Write(BinaryWriter w, NetVector v)
    {
        w.Write(v.x);
        w.Write(v.y);
        w.Write(v.z);
    }

    public static NetVector Read(BinaryReader r)
    {
        NetVector v = new NetVector();
        v.x = r.ReadSingle();
        v.y = r.ReadSingle();
        v.z = r.ReadSingle();
        return v;
    }

    public NetVector() { }

    public NetVector(Vector v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

public class NetFinger
{
    public NetBone[] bones;
    public int Type;


    public static void Write(BinaryWriter w, NetFinger f)
    {
        if (f.bones == null || f.bones.Length == 0)
        {
            w.Write(0);
        }
        else
        {
            w.Write(f.bones.Length);
            for (int i = 0; i < f.bones.Length; i++)
            {
                NetBone.Write(w, f.bones[i]);
            }
        }
        w.Write(f.Type);
    }

    public static NetFinger Read(BinaryReader r)
    {
        NetFinger f = new NetFinger();
        int count = r.ReadInt32();
        if (count > 0)
        {
            f.bones = new NetBone[count];
            for (int i = 0; i < f.bones.Length; i++)
            {
                f.bones[i] = NetBone.Read(r);
            }
        }
        f.Type = r.ReadInt32();
        return f;
    }

    public NetFinger() { }

    public NetFinger(Finger f)
    {
        bones = new NetBone[f.bones.Length];
        for (int i = 0; i < f.bones.Length; i++)
        {
            bones[i] = new NetBone(f.bones[i]);
        }
        Type = (int)(f.Type);
    }

    public NetBone Bone(int bonelx)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i].Type == bonelx)
            {
                return bones[i];
            }
        }
        return null;
    }
}

public class NetBone
{
    public NetVector NextJoint;
    public int Type;
    
    public static void Write(BinaryWriter w, NetBone b)
    {
        NetVector.Write(w, b.NextJoint);
        w.Write(b.Type);
    }

    public static NetBone Read(BinaryReader r)
    {
        NetBone b = new NetBone();
        b.NextJoint = NetVector.Read(r);
        b.Type = r.ReadInt32();
        return b;
    }

    public NetBone() { }

    public NetBone(Bone b)
    {
        NextJoint = new NetVector(b.NextJoint);
        Type = (int)(b.Type);
    }
}