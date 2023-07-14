using Xunit;

namespace Cake.Asciidoctor.Test;

/// <summary>
/// Tests for <see cref="AsciidoctorAttribute"/>
/// </summary>
public class AsciidoctorAttributeTest
{
    [Theory]
    [InlineData("NAME", "VALUE", "NAME=VALUE")]
    [InlineData("NAME", "VALUE WITH SPACES", "NAME=\"VALUE WITH SPACES\"")]
    [InlineData("name", "value", "name=value")]
    public void ToString_returns_expected_value_for_attribute_with_value(string name, string value, string expected)
    {
        // ARRANGE

        // ACT 
        var attribute = new AsciidoctorAttribute(name, value);

        // ASSERT
        Assert.Equal(expected, attribute.ToString());
    }

    [Theory]
    [InlineData("NAME", "NAME")]
    [InlineData("name", "name")]
    public void ToString_returns_expected_value_for_attribute_without_value(string name, string expected)
    {
        // ARRANGE

        // ACT 
        var attribute = new AsciidoctorAttribute(name);

        // ASSERT
        Assert.Equal(expected, attribute.ToString());
    }

    [Theory]
    [InlineData("NAME", "VALUE", "NAME@=VALUE")]
    [InlineData("NAME", "VALUE WITH SPACES", "NAME@=\"VALUE WITH SPACES\"")]
    [InlineData("name", "value", "name@=value")]
    public void ToString_returns_expected_value_for_non_overriding_attribute(string name, string value, string expected)
    {
        // ARRANGE

        // ACT 
        var attribute = new AsciidoctorAttribute(name, value)
        {
            Options = AsciidoctorAttributeOptions.NoOverride
        };

        // ASSERT
        Assert.Equal(expected, attribute.ToString());
    }


    [Theory]
    [InlineData("NAME", "VALUE", "NAME!")]
    [InlineData("NAME", "VALUE WITH SPACES", "NAME!")]
    [InlineData("name", "value", "name!")]
    public void ToString_returns_expected_value_for_unset_attribute(string name, string value, string expected)
    {
        // ARRANGE

        // ACT 
        var attribute = new AsciidoctorAttribute(name, value)
        {
            Unset = true
        };

        // ASSERT
        Assert.Equal(expected, attribute.ToString());
    }
}
