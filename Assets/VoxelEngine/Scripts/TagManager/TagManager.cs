using System.Collections.Generic;

public static class TagManager
{
	private static readonly Dictionary<TagType, string> _tags;

	static TagManager()
	{
		_tags = new Dictionary<TagType, string>
			{
				{TagType.Block, "Block"},
				{TagType.ComplexBlock, "ComplexBlock"}
			};
	}

	public static string GetTag(TagType tagType)
	{
		return _tags[tagType];
	}
}