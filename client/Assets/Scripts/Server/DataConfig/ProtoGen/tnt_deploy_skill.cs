//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: tnt_deploy_skill.proto
namespace tnt_deploy
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SKILL")]
  public partial class SKILL : global::ProtoBuf.IExtensible
  {
    public SKILL() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _name = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _template = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"template", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string template
    {
      get { return _template; }
      set { _template = value; }
    }
    private string _respath = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"respath", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string respath
    {
      get { return _respath; }
      set { _respath = value; }
    }
    private string _ui_skill_icon = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"ui_skill_icon", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string ui_skill_icon
    {
      get { return _ui_skill_icon; }
      set { _ui_skill_icon = value; }
    }
    private string _hit_efx = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"hit_efx", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string hit_efx
    {
      get { return _hit_efx; }
      set { _hit_efx = value; }
    }
    private uint _type = (uint)0;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)0)]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private uint _dir = (uint)0;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"dir", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)0)]
    public uint dir
    {
      get { return _dir; }
      set { _dir = value; }
    }
    private uint _avoid_method = (uint)0;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"avoid_method", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)0)]
    public uint avoid_method
    {
      get { return _avoid_method; }
      set { _avoid_method = value; }
    }
    private tnt_deploy.SKILL.velocity _efx_velocity = null;
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"efx_velocity", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public tnt_deploy.SKILL.velocity efx_velocity
    {
      get { return _efx_velocity; }
      set { _efx_velocity = value; }
    }
    private float _duration = (float)0;
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"duration", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float duration
    {
      get { return _duration; }
      set { _duration = value; }
    }
    private float _speed_scale = (float)0;
    [global::ProtoBuf.ProtoMember(12, IsRequired = false, Name=@"speed_scale", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float speed_scale
    {
      get { return _speed_scale; }
      set { _speed_scale = value; }
    }
    private float _chg_time = (float)0;
    [global::ProtoBuf.ProtoMember(13, IsRequired = false, Name=@"chg_time", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float chg_time
    {
      get { return _chg_time; }
      set { _chg_time = value; }
    }
    private float _continue_time = (float)0;
    [global::ProtoBuf.ProtoMember(14, IsRequired = false, Name=@"continue_time", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float continue_time
    {
      get { return _continue_time; }
      set { _continue_time = value; }
    }
    private float _reset_time = (float)0;
    [global::ProtoBuf.ProtoMember(15, IsRequired = false, Name=@"reset_time", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float reset_time
    {
      get { return _reset_time; }
      set { _reset_time = value; }
    }
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"velocity")]
  public partial class velocity : global::ProtoBuf.IExtensible
  {
    public velocity() {}
    
    private float _x = (float)0;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"x", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float x
    {
      get { return _x; }
      set { _x = value; }
    }
    private float _y = (float)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"y", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    [global::System.ComponentModel.DefaultValue((float)0)]
    public float y
    {
      get { return _y; }
      set { _y = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SKILL_ARRAY")]
  public partial class SKILL_ARRAY : global::ProtoBuf.IExtensible
  {
    public SKILL_ARRAY() {}
    
    private readonly global::System.Collections.Generic.List<tnt_deploy.SKILL> _items = new global::System.Collections.Generic.List<tnt_deploy.SKILL>();
    [global::ProtoBuf.ProtoMember(1, Name=@"items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<tnt_deploy.SKILL> items
    {
      get { return _items; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}