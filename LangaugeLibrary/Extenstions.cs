using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace LanguageLibrary
{
    public static class Extenstions
    {
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                var value = kvp.Value as IDictionary<string, object>;
                if (value != null)
                {
                    var expandoValue = value.ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else
                {
                    var items = kvp.Value as ICollection;
                    if (items != null)
                    {
                        // iterate through the collection and convert any strin-object dictionaries
                        // along the way into expando objects
                        var itemList = new List<object>();
                        foreach (var item in items)
                        {
                            var objects = item as IDictionary<string, object>;
                            if (objects != null)
                            {
                                var expandoItem = objects.ToExpando();
                                itemList.Add(expandoItem);
                            }
                            else
                            {
                                itemList.Add(item);
                            }
                        }

                        expandoDic.Add(kvp.Key, itemList);
                    }
                    else
                    {
                        expandoDic.Add(kvp);
                    }
                }
            }
            return expando;
        }
    }
}