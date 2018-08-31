//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Assets/ProtoBuffMsg/Rank.proto
namespace packet.rank
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankRequest")]
  public partial class RankRequest : global::ProtoBuf.IExtensible
  {
    public RankRequest() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankSyn")]
  public partial class RankSyn : global::ProtoBuf.IExtensible
  {
    public RankSyn() {}
    
    private readonly global::System.Collections.Generic.List<packet.rank.RankItem> _coinList = new global::System.Collections.Generic.List<packet.rank.RankItem>();
    [global::ProtoBuf.ProtoMember(2, Name=@"coinList", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<packet.rank.RankItem> coinList
    {
      get { return _coinList; }
    }
  
    private readonly global::System.Collections.Generic.List<packet.rank.RankItem> _gameCountList = new global::System.Collections.Generic.List<packet.rank.RankItem>();
    [global::ProtoBuf.ProtoMember(1, Name=@"gameCountList", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<packet.rank.RankItem> gameCountList
    {
      get { return _gameCountList; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankItem")]
  public partial class RankItem : global::ProtoBuf.IExtensible
  {
    public RankItem() {}
    
    private int _rank = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rank", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int rank
    {
      get { return _rank; }
      set { _rank = value; }
    }
    private int _playerId = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"playerId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int playerId
    {
      get { return _playerId; }
      set { _playerId = value; }
    }
    private string _playerName = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"playerName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string playerName
    {
      get { return _playerName; }
      set { _playerName = value; }
    }
    private string _playerHeadImg = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"playerHeadImg", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string playerHeadImg
    {
      get { return _playerHeadImg; }
      set { _playerHeadImg = value; }
    }
    private int _point = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"point", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int point
    {
      get { return _point; }
      set { _point = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}