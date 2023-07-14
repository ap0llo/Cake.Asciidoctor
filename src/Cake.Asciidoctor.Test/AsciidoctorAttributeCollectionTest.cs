using System;
using System.Linq;
using Xunit;

namespace Cake.Asciidoctor.Test;

/// <summary>
/// Tests for <see cref="AsciidoctorAttributeCollection"/>
/// </summary>
public class AsciidoctorAttributeCollectionTest
{
    [Fact]
    public void Attribute_value_can_be_set_via_indexer()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection();

        // ACT
        attributes["Name"] = "Value";

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name=Value", x.ToString()));
    }

    [Fact]
    public void Attributes_can_be_added_via_collection_initializer()
    {
        // ARRANGE / ACT
        var attributes = new AsciidoctorAttributeCollection()
        {
            { "Name1", "Value1" },
            { "Name2", "Value2" },
            { "Name3" }
        };

        // ASSERT
        Assert.Collection(
            attributes.OrderBy(x => x.Name, StringComparer.Ordinal),
            x => Assert.Equal("Name1=Value1", x.ToString()),
            x => Assert.Equal("Name2=Value2", x.ToString()),
            x => Assert.Equal("Name3", x.ToString())
        );
    }

    [Fact]
    public void Define_adds_attribute_with_empty_value()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection();

        // ACT
        attributes.Define("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name", x.ToString()));
    }

    [Fact]
    public void Define_removes_previously_added_value()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection()
        {
            { "Name", "Value" }
        };

        // ACT
        attributes.Define("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name", x.ToString()));
    }

    [Fact]
    public void Define_removes_Unset_flag()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection();
        attributes.Unset("Name");

        // ACT
        attributes.Define("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name", x.ToString()));
    }

    [Theory]
    [InlineData(null, "Name")]
    [InlineData("some-value", "Name=some-value")]
    public void Setting_value_removes_Unset_flag(string? value, string expected)
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection();
        attributes.Unset("Name");

        // ACT
        attributes["Name"] = value;

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal(expected, x.ToString()));
    }


    [Fact]
    public void Unset_adds_attribute_with_unset_option()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection();

        // ACT
        attributes.Unset("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name!", x.ToString()));
    }

    [Fact]
    public void Unset_removes_previously_added_value()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection()
        {
            { "Name", "Value" }
        };

        // ACT
        attributes.Unset("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name!", x.ToString()));
    }

    [Fact]
    public void Unset_removes_previously_set_non_override_option()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection()
        {
            { "Name", "Value", AsciidoctorAttributeOptions.NoOverride }
        };

        // ACT
        attributes.Unset("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name!", x.ToString()));
    }

    [Fact]
    public void Add_non_overriding_attribute()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection
        {
            { "Name", "Value", AsciidoctorAttributeOptions.NoOverride }
        };

        // ACT
        attributes.Unset("Name");

        // ASSERT
        Assert.Collection(attributes, x => Assert.Equal("Name!", x.ToString()));
    }

    [Fact]
    public void Remove_removes_attribute_from_collection()
    {
        // ARRANGE
        var attributes = new AsciidoctorAttributeCollection
        {
            { "Name", "Value" }
        };

        // ACT
        attributes.Remove("Name");

        // ASSERT
        Assert.Empty(attributes);
    }
}
