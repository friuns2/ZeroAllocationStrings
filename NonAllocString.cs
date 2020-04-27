//example for(int i=0;i<1000;i++) string newZeroGcString = NonAllocString.instance+"test"+23;

using System;
using System.Collections.Generic;
using System.Text;


public class NonAllocString
{
    private NonAllocString()
    {
        
    }
    public static NonAllocString instance
    {
        get
        {
            if (m_instance.Count > 0)
            {
                var nonAllocString = m_instance.Pop();
                nonAllocString.generatedString = null;
                return nonAllocString;
            }
            else
                return new NonAllocString();
        }
    }        

    static readonly Stack<NonAllocString> m_instance = new Stack<NonAllocString>();
    List<AnyValue> anyValues = new List<AnyValue>();
    public static NonAllocString operator +(NonAllocString a, object input)
    {
        a.anyValues.Add(new AnyValue() {type = input?.GetType(), obj = input is NonAllocString d ? (string)d : input is string s ? s : input});
        return a;
    }
    
    public static NonAllocString operator +(NonAllocString a, int input)
    {
        a.anyValues.Add(input);
        return a;
    }

    public static NonAllocString operator +(NonAllocString a, float input)
    {
        a.anyValues.Add(input);
        return a;
    }

    public StringBuilder sb = new StringBuilder();
    public string GetString()
    {
        sb.Clear();
        for (int i = 0; i < anyValues.Count; i++)
        {
            if (anyValues[i].obj != null)
                sb.Append(anyValues[i].obj);
            else if (anyValues[i].type == typeof(int))
                sb.Append(anyValues[i].integer);
            else if (anyValues[i].type == typeof(float))
                sb.Append(anyValues[i].float2);
              
        }
        return sb.ToString();
    }
    public override string ToString()
    {
        return Dispose();
    }
    public static implicit operator string(NonAllocString c)
    {
        return c.Dispose();
    }
    private string Dispose()
    {
        if (!string.IsNullOrEmpty(generatedString)) 
            return generatedString;

        var key = ArrayHash();
        string s;
        if (!stringcache.TryGetValue(key, out s))
            s = stringcache[key] =  GetString();

        m_instance.Push(this);
        anyValues.Clear();
        return generatedString = s;
    }
    private string generatedString;
    
    public static Dictionary<int, string> stringcache = new Dictionary<int, string>();

    public int ArrayHash()
    {

        int key = 123;
        for (int i = 0; i < anyValues.Count; i++)
        {
            AnyValue o = anyValues[i];
            key = CombineHash(key, o.GetHashCode());
        }
        return key;
    }

    public static int CombineHash(int h1, int h2)
    {
        return ((h1 << 5) + h1) ^ h2;
    }
    struct AnyValue
    {
        public Type type;
        public object obj;
        public int integer;
        public float float2;

    
        public static implicit operator int(AnyValue msg)
        {
            return msg.integer;
        }
        public static implicit operator AnyValue(int c)
        {
            return new AnyValue() {integer = c,type = typeof(int)};
        }
        public static implicit operator float(AnyValue msg)
        {
            return msg.float2;
        }
        public static implicit operator AnyValue(float c)
        {
            return new AnyValue() {float2 = c, type = typeof(float)};
        }
        public override int GetHashCode()
        {
            if (obj != null) return CombineHash(type.GetHashCode(), obj.GetHashCode());
            if (type == typeof(int)) return CombineHash(integer.GetHashCode(), 2345);
            if (type == typeof(float)) return CombineHash(float2.GetHashCode(), 4933);
            
            throw new Exception("dad");
        }
    }

}
