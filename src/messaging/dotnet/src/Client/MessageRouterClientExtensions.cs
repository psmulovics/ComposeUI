﻿// Morgan Stanley makes this available to you under the Apache License,
// Version 2.0 (the "License"). You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0.
// 
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership. Unless required by applicable law or agreed
// to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
// or implied. See the License for the specific language governing permissions
// and limitations under the License.

using MorganStanley.ComposeUI.Messaging.Client;

namespace MorganStanley.ComposeUI.Messaging;

/// <summary>
///     Provides extensions methods for <see cref="IMessageRouter" />
/// </summary>
public static class MessageRouterClientExtensions
{
    public static IAsyncObservable<TopicMessage> Topic(this IMessageRouter messageRouter, string topic)
    {
        return messageRouter is MessageRouterClient messageRouterClient
            ? messageRouterClient.GetTopicObservable(topic)
            : new TopicObservable(messageRouter, topic);
    }

    private sealed class TopicObservable : IAsyncObservable<TopicMessage>
    {
        public TopicObservable(IMessageRouter messageRouter, string topic)
        {
            _messageRouter = messageRouter;
            _topic = topic;
        }

        public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<TopicMessage> observer)
        {
            return _messageRouter.SubscribeAsync(_topic, observer, CancellationToken.None);
        }

        private readonly IMessageRouter _messageRouter;
        private readonly string _topic;
    }
}