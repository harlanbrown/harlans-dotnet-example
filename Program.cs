using System;
using System.Threading.Tasks;
using NuxeoClient;
using NuxeoClient.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestNuxeoApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// create a new Nuxeo Client, which is a disposable objects
			using(var client = new Client(Config.ServerUrl()))
			{
				// let's create an async job that doesn't block
				System.Threading.Tasks.Task.Run(async () => {
					// perform an async request to create a folder on the root with the title "My Folder"
					Document folder = (Document)await client.Operation("Document.Create")
															.SetInput("doc:/")
															.SetParameter("type", "Folder")
															.SetParameter("name", "MyFolder")
															.SetParameter("properties", new ParamProperties { { "dc:title", "My Folder" } })
															.Execute();

					// print the returned folder object if it is not null			
					Console.WriteLine(folder == null ? "Failed to create folder." : "Created " + folder.Path);

                    // perform an async request to create a child file named "My File"						
//Document file = (Document)await folder.Post(new Document
//{
//    Type = "File",
//     Name = "MyFile",
//     Properties = new Properties { { "dc:title", "My File" } }
// });

// file = (Document)await client.Operation("Document.AddFacet")
//                              .SetInput(file.Uid)
//                              .SetParameter("facet", "facet1")
//                              .SetParameter("save", "true")
//                              .Execute();

// file = (Document)await client.Operation("Document.AddFacet")
//                              .SetInput(file.Uid)
//                              .SetParameter("facet", "facet2")
//                              .SetParameter("save", "true")
//                              .Execute();

// file = (Document)await client.Operation("Document.AddFacet")
//                              .SetInput(file.Uid)
//                              .SetParameter("facet", "myFacet")
//                              .SetParameter("save", "true")
//                              .Execute();

// file = (Document)await client.Operation("Document.SetProperty")
//                              .SetInput(file.Uid)
//                              .SetParameter("xpath", "schema2:prop2")
//                              .SetParameter("value", 1)
//                              .SetParameter("save", "true")
//                              .Execute();

string facets = JsonConvert.SerializeObject(new string[]{"facet1","facet2","myFacet"});

string props = JsonConvert.SerializeObject(new ParamProperties {{"schema2:prop2",1},{"schema1:prop1","prop"}});    
Console.WriteLine(props);

Document file = (Document)await client.Operation("javascript.myScript")
                                      .SetInput(folder.Uid)
                                      .SetParameter("docName", "my other File")
                                      .SetParameter("facets", facets)
                                      .SetParameter("props", props)
                                      .Execute();

                    // print the returned file object if it is not null					   
                    Console.WriteLine(file == null ? "Failed to create file." : "Created " + file.Path);
				}).Wait(); // let's wait for the task to complete
			}
		}
	}
}
