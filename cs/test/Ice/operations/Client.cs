// **********************************************************************
//
// Copyright (c) 2003
// ZeroC, Inc.
// Billerica, MA, USA
//
// All Rights Reserved.
//
// Ice is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License version 2 as published by
// the Free Software Foundation.
//
// **********************************************************************

using System;

public class Client
{
    private static int run(String[] args, Ice.Communicator communicator)
    {
	Test.MyClassPrx myClass = AllTests.allTests(communicator, false);
	
	Console.Out.Write("testing server shutdown... ");
	Console.Out.Flush();
	myClass.shutdown();
	try
	{
	    myClass.opVoid();
	    throw new System.Exception();
	}
	catch(Ice.LocalException)
	{
	    Console.Out.WriteLine("ok");
	}
	
	return 0;
    }
    
    public static void Main(string[] args)
    {
	int status = 0;
	Ice.Communicator communicator = null;
	
	try
	{
	    Ice.Properties properties = Ice.Util.getDefaultProperties(ref args);
	    properties.setProperty("Ice.ThreadPool.Client.Size", "2"); // For nested AMI.
	    properties.setProperty("Ice.ThreadPool.Client.SizeWarn", "0");

	    communicator = Ice.Util.initialize(ref args);
	    status = run(args, communicator);
	}
	catch(System.Exception ex)
	{
	    Console.Error.WriteLine(ex);
	    status = 1;
	}
	
	if(communicator != null)
	{
	    try
	    {
		communicator.destroy();
	    }
	    catch(Ice.LocalException ex)
	    {
		Console.Error.WriteLine(ex);
		status = 1;
	    }
	}
	
	Environment.Exit(status);
    }
}
