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
    public NetArm Arm;
    public float Confidence;
    public NetVector Direction;
    public List<NetFinger> Fingers;
    public long FrameId;
    public float GrabAngle;
    public float GrabStrength;
    public int Id;
    public bool IsLeft;
    public NetVector PalmNormal;
    public NetVector PalmPosition;
    public NetVector PalmVelocity;
    public float PalmWidth;
    public float PinchDistance;
    public float PinchStrength;
    public NetVector StabilizedPalmPosition;
    public float TimeVisible;
    public NetVector WristPosition;
    public NetTransform Basis;

    public static void Write(BinaryWriter w, NetHand h)
    {
        NetArm.Write(w, h.Arm);
        w.Write(h.Confidence);
        NetVector.Write(w, h.Direction);
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
        w.Write(h.FrameId);
        w.Write(h.GrabAngle);
        w.Write(h.GrabStrength);
        w.Write(h.Id);
        w.Write(h.IsLeft);
        NetVector.Write(w, h.PalmNormal);
        NetVector.Write(w, h.PalmPosition);
        NetVector.Write(w, h.PalmVelocity);
        w.Write(h.PalmWidth);
        w.Write(h.PinchDistance);
        w.Write(h.PinchStrength);
        NetVector.Write(w, h.StabilizedPalmPosition);
        w.Write(h.TimeVisible);
        NetVector.Write(w, h.WristPosition);
        NetTransform.Write(w, h.Basis);
    }

    public static NetHand Read(BinaryReader r)
    {
        NetHand h = new NetHand();
        h.Arm = NetArm.Read(r);
        h.Confidence = r.ReadSingle();
        h.Direction = NetVector.Read(r);
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
        h.FrameId = r.ReadInt64();
        h.GrabAngle = r.ReadSingle();
        h.GrabStrength = r.ReadSingle();
        h.Id = r.ReadInt32();
        h.IsLeft = r.ReadBoolean();
        h.PalmNormal = NetVector.Read(r);
        h.PalmPosition = NetVector.Read(r);
        h.PalmVelocity = NetVector.Read(r);
        h.PalmWidth = r.ReadSingle();
        h.PinchDistance = r.ReadSingle();
        h.PinchStrength = r.ReadSingle();
        h.StabilizedPalmPosition = NetVector.Read(r);
        h.TimeVisible = r.ReadSingle();
        h.WristPosition = NetVector.Read(r);
        h.Basis = NetTransform.Read(r);

        return h;
    }

    public NetHand() { }

    public NetHand(Hand h)
    {
        Arm = new NetArm(h.Arm);
        Confidence = h.Confidence;
        Direction = new NetVector(h.Direction);
        Fingers = new List<NetFinger>();
        foreach (Finger f in h.Fingers)
        {
            Fingers.Add(new NetFinger(f));
        }
        FrameId = h.FrameId;
        GrabAngle = h.GrabAngle;
        GrabStrength = h.GrabStrength;
        Id = h.Id;
        IsLeft = h.IsLeft;
        PalmNormal = new NetVector(h.PalmNormal);
        PalmPosition = new NetVector(h.PalmPosition);
        PalmVelocity = new NetVector(h.PalmVelocity);
        PalmWidth = h.PalmWidth;
        PinchDistance = h.PinchDistance;
        PinchStrength = h.PinchStrength;
        StabilizedPalmPosition = new NetVector(h.StabilizedPalmPosition);
        TimeVisible = h.TimeVisible;
        WristPosition = new NetVector(h.WristPosition);
        Basis = new NetTransform(h.Basis);
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
    public NetVector Direction;
    public int HandId;
    public int Id;
    public bool IsExtended;
    public float Length;
    public NetVector StabilizedTipPosition;
    public float TimeVisible;
    public NetVector TipPosition;
    public NetVector TipVelocity;
    public int Type;
    public float Width;


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
        NetVector.Write(w, f.Direction);
        w.Write(f.HandId);
        w.Write(f.Id);
        w.Write(f.IsExtended);
        w.Write(f.Length);
        NetVector.Write(w, f.StabilizedTipPosition);
        w.Write(f.TimeVisible);
        NetVector.Write(w, f.TipPosition);
        NetVector.Write(w, f.TipVelocity);
        w.Write(f.Type);
        w.Write(f.Width);
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
        f.Direction = NetVector.Read(r);
        f.HandId = r.ReadInt32();
        f.Id = r.ReadInt32();
        f.IsExtended = r.ReadBoolean();
        f.Length = r.ReadSingle();
        f.StabilizedTipPosition = NetVector.Read(r);
        f.TimeVisible = r.ReadSingle();
        f.TipPosition = NetVector.Read(r);
        f.TipVelocity = NetVector.Read(r);
        f.Type = r.ReadInt32();
        f.Width = r.ReadSingle();
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
        Direction = new NetVector(f.Direction);
        HandId = f.HandId;
        Id = f.Id;
        IsExtended = f.IsExtended;
        Length = f.Length;
        StabilizedTipPosition = new NetVector(f.StabilizedTipPosition);
        TimeVisible = f.TimeVisible;
        TipPosition = new NetVector(f.TipPosition);
        TipVelocity = new NetVector(f.TipVelocity);
        Type = (int)(f.Type);
        Width = f.Width;
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

public class NetArm
{
    public NetVector Center;
    public NetVector Direction;
    public float Length;
    public NetVector ElbowPosition;
    public NetVector WristPosition;
    public NetLeapQuaternion Rotation;
    public int Type;
    public float Width;
    public NetTransform Basis;

    public static void Write(BinaryWriter w, NetArm b)
    {
        NetVector.Write(w, b.Center);
        NetVector.Write(w, b.Direction);
        w.Write(b.Length);
        NetVector.Write(w, b.ElbowPosition);
        NetVector.Write(w, b.WristPosition);
        NetLeapQuaternion.Write(w, b.Rotation);
        w.Write(b.Type);
        w.Write(b.Width);
        NetTransform.Write(w, b.Basis);
    }

    public static NetArm Read(BinaryReader r)
    {
        NetArm b = new NetArm();
        b.Center = NetVector.Read(r);
        b.Direction = NetVector.Read(r);
        b.Length = r.ReadSingle();
        b.ElbowPosition = NetVector.Read(r);
        b.WristPosition = NetVector.Read(r);
        b.Rotation = NetLeapQuaternion.Read(r);
        b.Type = r.ReadInt32();
        b.Width = r.ReadSingle();
        b.Basis = NetTransform.Read(r);
        return b;
    }

    public NetArm() { }

    public NetArm(Arm a)
    {
        Center = new NetVector(a.Center);
        Direction = new NetVector(a.Direction);
        Length = a.Length;
        ElbowPosition = new NetVector(a.ElbowPosition);
        WristPosition = new NetVector(a.WristPosition);
        Rotation = new NetLeapQuaternion(a.Rotation);
        Type = (int)(a.Type);
        Width = a.Width;
        Basis = new NetTransform(a.Basis);
    }
}

public class NetBone
{
    public NetVector Center;
    public NetVector Direction;
    public float Length;
    public NetVector NextJoint;
    public NetVector PrevJoint;
    public NetLeapQuaternion Rotation;
    public int Type;
    public float Width;
    public NetTransform Basis;
    
    public static void Write(BinaryWriter w, NetBone b)
    {
        NetVector.Write(w, b.Center);
        NetVector.Write(w, b.Direction);
        w.Write(b.Length);
        NetVector.Write(w, b.NextJoint);
        NetVector.Write(w, b.PrevJoint);
        NetLeapQuaternion.Write(w, b.Rotation);
        w.Write(b.Type);
        w.Write(b.Width);
        NetTransform.Write(w, b.Basis);
    }

    public static NetBone Read(BinaryReader r)
    {
        NetBone b = new NetBone();
        b.Center = NetVector.Read(r);
        b.Direction = NetVector.Read(r);
        b.Length = r.ReadSingle();
        b.NextJoint = NetVector.Read(r);
        b.PrevJoint = NetVector.Read(r);
        b.Rotation = NetLeapQuaternion.Read(r);
        b.Type = r.ReadInt32();
        b.Width = r.ReadSingle();
        b.Basis = NetTransform.Read(r);
        return b;
    }

    public NetBone() { }

    public NetBone(Bone b)
    {
        Center = new NetVector(b.Center);
        Direction = new NetVector(b.Direction);
        Length = b.Length;
        NextJoint = new NetVector(b.NextJoint);
        PrevJoint = new NetVector(b.PrevJoint);
        Rotation = new NetLeapQuaternion(b.Rotation);
        Type = (int)(b.Type);
        Width = b.Width;
        Basis = new NetTransform(b.Basis);
    }
}

public class NetLeapQuaternion
{
    public float w;
    public float x;
    public float y;
    public float z;
    
    public static void Write(BinaryWriter w, NetLeapQuaternion v)
    {
        w.Write(v.w);
        w.Write(v.x);
        w.Write(v.y);
        w.Write(v.z);
    }

    public static NetLeapQuaternion Read(BinaryReader r)
    {
        NetLeapQuaternion v = new NetLeapQuaternion();
        v.w = r.ReadSingle();
        v.x = r.ReadSingle();
        v.y = r.ReadSingle();
        v.z = r.ReadSingle();
        return v;
    }

    public NetLeapQuaternion() { }

    public NetLeapQuaternion(LeapQuaternion q)
    {
        w = q.w;
        x = q.x;
        y = q.y;
        z = q.z;
    }
}

public class NetTransform
{
    public NetLeapQuaternion rotation;
    public NetVector scale;
    public NetVector translation;
    public NetVector xBasis;
    public NetVector yBasis;
    public NetVector zBasis;
    
    public static void Write(BinaryWriter w, NetTransform v)
    {
        NetLeapQuaternion.Write(w, v.rotation);
        NetVector.Write(w, v.scale);
        NetVector.Write(w, v.translation);
        NetVector.Write(w, v.xBasis);
        NetVector.Write(w, v.yBasis);
        NetVector.Write(w, v.zBasis);
    }

    public static NetTransform Read(BinaryReader r)
    {
        NetTransform v = new NetTransform();
        v.rotation = NetLeapQuaternion.Read(r);
        v.scale = NetVector.Read(r);
        v.translation = NetVector.Read(r);
        v.xBasis = NetVector.Read(r);
        v.yBasis = NetVector.Read(r);
        v.zBasis = NetVector.Read(r);
        return v;
    }

    public NetTransform() { }


    public NetTransform(LeapTransform lt)
    {
        rotation = new NetLeapQuaternion(lt.rotation);
        scale = new NetVector(lt.scale);
        translation = new NetVector(lt.translation);
        xBasis = new NetVector(lt.xBasis);
        yBasis = new NetVector(lt.yBasis);
        zBasis = new NetVector(lt.zBasis);
    }
}