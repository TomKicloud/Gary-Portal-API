using System;
using GaryPortalAPI.Models.Feed;
using Newtonsoft.Json.Linq;

namespace GaryPortalAPI.Data
{
    public class FeedPostJsonConverter : JsonCreationConverter<FeedPost>
    {
        protected override FeedPost Create(Type objectType, JObject jObject)
        {
            if (jObject == null)
                throw new ArgumentNullException(nameof(jObject));

            if (jObject.GetValue("postType").ToString().Equals("media"))
                return new FeedMediaPost();
            else if (jObject.GetValue("postType").ToString().Equals("poll"))
                return new FeedPollPost();
            else
                return new FeedPost();
        }
    }
}
