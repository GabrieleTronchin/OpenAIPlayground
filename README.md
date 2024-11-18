# OpenAI Playground

This repository is a playground to test some integration between C# and OpenAI. It uses the Azure OpenAI SDK.

## OpenAI SDK Integration

To integrate OpenAI, we use the Azure OpenAI SDK. This SDK provides the `ChatClient` class, which enables us to interact with OpenAI's chat-based models for generating responses.

[Azure OpenAI SDK Documentation](https://azuresdkdocs.blob.core.windows.net/$web/dotnet/Azure.AI.OpenAI/1.0.0-beta.8/index.html)

## Basic Concepts about OpenAI

### Models

The first thing to know is how to choose a model. OpenAI offers many different models. Some famous models you may have already heard about are GPT-3, GPT-3.5, and GPT-4.  
You can find descriptions of the latest models available on Azure at the following link:

[OpenAI Models on Azure](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models?tabs=python-secure%2Cglobal-standard%2Cstandard-chat-completions)

### Tokens

In OpenAI's context, a "token" is a fundamental unit of text processing used by their language models, such as GPT-3 and GPT-4.

A token is the smallest unit of text that the model processes. Common words are typically one token, while complex or less common words may be broken into multiple tokens.

### Prompts

A ChatGPT prompt is an instruction or topic provided by a user to the ChatGPT AI model. It can take various forms, such as questions, statements, or scenarios, and is intended to stimulate creativity, reflection, or engagement from the AI.

## Project Samples

This project contains some samples:
- Summarize a text in different languages.
- Summarize reviews in different languages.
- Create a sample factory using GPT that retrieves different data according to user questions.

NOTE: This README is in progress.
