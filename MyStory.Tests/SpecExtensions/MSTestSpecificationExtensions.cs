using System;
using System.Collections;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public static class MsTestSpecificationExtensions
{
    public static void ShouldBeFalse(this bool condition, string message = "")
    {
        Assert.IsFalse(condition, message);
    }


    public static void ShouldBeTrue(this bool condition, string message = "")
    {
        Assert.IsTrue(condition, message);
    }

    public static T ShouldEqual<T>(this T actual, T expected, string message = "")
    {
        if (actual == null)
        {
            Assert.AreEqual(expected, actual, message);
        }
        else if (actual.GetType().BaseType != null && actual.GetType().BaseType != typeof(System.Array))
            Assert.AreEqual(expected, actual, message);
        else
            CollectionAssert.AreEqual((ICollection)expected, (ICollection)actual, message: message);
        return actual;
    }

    public static T ShouldNotEqual<T>(this T actual, T expected, string message = "")
    {
        if (actual == null)
        {
            Assert.AreNotEqual(expected, actual, message);
        }
        else if (actual.GetType().BaseType != null && actual.GetType().BaseType != typeof(System.Array))
            Assert.AreNotEqual(expected, actual, message);
        else
            CollectionAssert.AreNotEqual((ICollection)expected, (ICollection)actual, message: message);
        return actual;
    }

    public static string ShouldEqual(this string actual, string expected, bool ignoreCase)
    {
        Assert.AreEqual(expected, actual, ignoreCase);
        return actual;
    }

    //public static dynamic ShouldEqual(this dynamic actual, string expected, bool ignoreCase)
    //{
    //    Assert.AreEqual(expected, actual, ignoreCase);
    //    return actual;
    //}


    public static string ShouldNotEqual(this string actual, string notExpected, bool ignoreCase = false, string message = "")
    {
        Assert.AreNotEqual(notExpected, actual, ignoreCase, message);
        return actual;
    }


    public static void ShouldBeNull(this object anObject, string message)
    {
        Assert.IsNull(anObject, message);
        // don't return anything here--it is supposed to be null
    }

    public static void ShouldBeNull(this object anObject)
    {
        Assert.IsNull(anObject);
    }

    public static T ShouldNotBeNull<T>(this T anObject)
    {
        return ShouldNotBeNull(anObject, string.Empty);
    }

    public static T ShouldNotBeNull<T>(this T anObject, string message)
    {
        Assert.IsNotNull(anObject, message);
        return anObject;
    }


    public static T ShouldBeTheSameAs<T>(this T actual, T expected)
    {
        Assert.AreSame(expected, actual);
        return actual;
    }



    public static T ShouldNotBeTheSameAs<T>(this T actual, T notExpected)
    {
        Assert.AreNotSame(notExpected, actual);
        return actual;
    }



    public static T ShouldBeOfType<T>(this T actual, Type expectedType)
    {
        Assert.IsInstanceOfType(actual, expectedType);
        return actual;
    }


    public static T ShouldNotBeOfType<T>(this T actual, Type wrongType)
    {
        Assert.IsNotInstanceOfType(actual, wrongType);
        return actual;
    }


    public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2, string message = "")
    {
        if (arg1.CompareTo(arg2) <= 0)
            Assert.Fail(string.Format(CultureInfo.CurrentCulture, "Value {0} is not greater than {1}! - {2}", arg1, arg2, message));

        return arg1;
    }

    public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
    {
        if (arg1.CompareTo(arg2) >= 0)
            Assert.Fail(string.Format(CultureInfo.CurrentCulture, "Value {0} is not less than {1}!", arg1, arg2));

        return arg1;
    }


    public static IEnumerable ShouldBeEmpty(this IEnumerable collection)
    {
        Assert.IsFalse(HasAnyElements(collection));
        return collection;
    }

    private static bool HasAnyElements(IEnumerable collection)
    {
        IEnumerator enumerator = collection.GetEnumerator();
        return enumerator.MoveNext();
    }

    public static IEnumerable ShouldNotBeEmpty(this IEnumerable collection)
    {
        Assert.IsTrue(HasAnyElements(collection));
        return collection;
    }


    public static ICollection ShouldContain(this ICollection collection, object element)
    {
        CollectionAssert.Contains(collection, element);
        return collection;
    }


    public static ICollection ShouldNotContain(this ICollection collection, object element)
    {
        CollectionAssert.DoesNotContain(collection, element);
        return collection;
    }


    public static string ShouldContain(this string value, string substring)
    {
        StringAssert.Contains(value, substring);
        return value;
    }


    public static string ShouldNotContain(this string value, string substring)
    {
        if (value == null && substring == null)
            Assert.Fail("Null string 'contains' null string!");

        // this is a very weird case, but if the value is null, then it doesn't really contain a non-null substring
        if (value == null)
            return value;

        bool contains = value.Contains(substring);
        Assert.IsFalse(contains);

        return value;
    }


    public static Exception AndExceptionMessageShouldContain(this Exception exception, string expected)
    {
        if (exception == null)
            throw new ArgumentNullException("exception");

        exception.Message.ShouldContain(expected);
        return exception;
    }

    public static Exception ShouldBeThrownBy(this Type exceptionType, Action method)
    {
        Exception exception = method.GetException();

        Assert.IsNotNull(exception, "No exception was thrown!");

        Assert.AreEqual(exceptionType, exception.GetType(), "The wrong type of exception was thrown!");

        return exception;
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public static Exception GetException(this Action method)
    {
        Exception exception = null;

        try
        {
            method();
        }
        catch (Exception e)
        {
            exception = e;
        }

        return exception;
    }




    public static T ShouldBeInRange<T>(this T actual, T low, T high) where T : IComparable
    {
        bool isInRange = actual.IsInRange(low, high);

        Assert.IsTrue(isInRange, string.Format(CultureInfo.CurrentCulture, "Value {0} was not in the expected range of {1} to {2}!", actual, low, high));

        return actual;
    }

    private static bool IsInRange<T>(this T actual, T low, T high) where T : IComparable
    {
        if (actual.CompareTo(low) < 0)
            return false;

        if (actual.CompareTo(high) > 0)
            return false;

        return true;
    }


    public static T ShouldNotBeInRange<T>(this T actual, T low, T high) where T : IComparable
    {
        bool isInRange = actual.IsInRange(low, high);

        Assert.IsFalse(isInRange, string.Format(CultureInfo.CurrentCulture, "Value {0} was in the unexpected range of {1} to {2}!", actual, low, high));

        return actual;
    }



    public static string ShouldEqualIgnoringCase(this string actual, string expected)
    {
        string actualLower = actual.ToLower();
        string expectedLower = expected.ToLower();

        actualLower.ShouldEqual(expectedLower);

        return actual;
    }

    public static string ShouldStartWith(this string actual, string expected)
    {
        StringAssert.StartsWith(expected, actual);
        return actual;
    }

    public static string ShouldEndWith(this string actual, string expected)
    {
        StringAssert.EndsWith(expected, actual);
        return actual;
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
    {
        StringAssert.StartsWith(expectedStartDelimiter, actual);
        StringAssert.EndsWith(expectedEndDelimiter, actual);
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
    {
        StringAssert.StartsWith(expectedDelimiter, actual);
        StringAssert.EndsWith(expectedDelimiter, actual);
    }
}
