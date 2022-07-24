BE 
Install AWSToolkitPackage.v17.vsix
Create New Project 
Choose Template AWS Serverless  Application .NET Core - C# 
Select Blueprint ASP.NET CORE WEB API
After Write Api In controller Production -> 
+API GET 1 PRODUCTION - /api/production/ID Method Get
+API GET ALL PRODUCTION - /api/production/  Method Get
+API INSERT 1 PRODUCTION - /api/production/  Method POST 
Payload Request EX:
{
"Id":"005",
"Name":"Sản phẩm 005",
"Image":"https://media-cdn.tripadvisor.com/media/photo-s/1e/18/dc/e4/vu-n-hoa-trung-tam-happy.jpg",
"Price":70000
}

+API Update 1 PRODUCTION Method PUT 
{
"Id":"005",
"Name":"Sản phẩm 005",
"Image":"https://media-cdn.tripadvisor.com/media/photo-s/1e/18/dc/e4/vu-n-hoa-trung-tam-happy.jpg",
"Price":70000
}
+API DETELE PRODUCTION  - /api/production?Id=001&Name=Sản phẩm 001  Method DELETE 

+API UPLOAD IMAGE S3 -api/production/UploadFiles Method POST
Form-data : files,id,name


Setting StartUp Can Run API In ConfigureServices
Write Serveless Create Table Production with 
"DynamoDBTable": {
            "Type": "AWS::DynamoDB::Table",
            "Properties": {
                "AttributeDefinitions": [
                    {
                        "AttributeName": "Id",
                        "AttributeType": "S"
                    },
                    {
                        "AttributeName": "Name",
                        "AttributeType": "S"
                    }
                ],
                "KeySchema": [
                    {
                        "AttributeName": "Id",
                        "KeyType": "HASH"
                    },
                    {
                        "AttributeName": "Name",
                        "KeyType": "RANGE"
                    }
                ],
                "TableName": "Production",
                "ProvisionedThroughput": {
                    "ReadCapacityUnits": 10,
                    "WriteCapacityUnits": 10
                }
            }
        }

Setting S3 
 "AttachmentsBucket": {
            "Type": "AWS::S3::Bucket",
            "Properties": {
                "BucketName": "bucket-pro5",
                "AccessControl": "PublicRead",
                "CorsConfiguration": {
                    "CorsRules": [
                        {
                            "AllowedHeaders": [
                                "*"
                            ],
                            "AllowedMethods": [
                                "PUT",
                                "POST",
                                "DELETE",
                                "GET",
                                "HEAD"
                            ],
                            "AllowedOrigins": [
                                "*"
                            ]
                        }
                    ]
                }
            }
        },
        "BucketPolicy": {
            "Type": "AWS::S3::BucketPolicy",
            "Properties": {
                "PolicyDocument": {
                    "Version": "2012-10-17",
                    "Statement": {
                        "Sid": "PublicReadForGetBucketObjects",
                        "Effect": "Allow",
                        "Principal": "*",
                        "Action": "*",
                        "Resource": "arn:aws:s3:::bucket-pro5/*"
                    }
                },
                "Bucket": {
                    "Ref": "AttachmentsBucket"
                }
            }
        }

Setting API Function 
Enable Cors 
 "ApiGatewayApi": {
            "Type": "AWS::Serverless::Api",
            "Properties": {
                "StageName": "Prod",
                "Cors": "'https://localhost:59161,https://localhost:59161;http://localhost:59162'"
            }
        },
        "AspNetCoreFunction": {
            "Type": "AWS::Serverless::Function",
            "Properties": {
                "Handler": "Project5::Project5.LambdaEntryPoint::FunctionHandlerAsync",
                "Runtime": "dotnet6",
                "CodeUri": "",
                "MemorySize": 256,
                "Timeout": 30,
                "Role": null,
                "Policies": [
                    "AWSLambda_FullAccess",
                    "AmazonS3FullAccess"
                ],
                "Events": {
                    "ProxyResource": {
                        "Type": "Api",
                        "Properties": {
                            "Path": "/{proxy+}",
                            "Method": "ANY",
                            "RestApiId": {
                                "Ref": "ApiGatewayApi"
                            }
                        }
                    },
                    "RootResource": {
                        "Type": "Api",
                        "Properties": {
                            "Path": "/",
                            "Method": "ANY",
                            "RestApiId": {
                                "Ref": "ApiGatewayApi"
                            }
                        }
                    }
                },
                "FunctionUrlConfig": {
                    "AuthType": "NONE",
                    "Cors": {
                        "AllowCredentials": true,
                        "AllowMethods": [
                            "PUT",
                            "POST",
                            "DELETE",
                            "GET",
                            "HEAD"
                        ],
                        "AllowOrigins": [
                            "*"
                        ],
                        "AllowHeaders": [
                            "*"
                        ]
                    }
                }
            }
        }
I setting Enviroment Prod 
Endpoind: https://s6px12t3qe.execute-api.us-east-1.amazonaws.com/Prod/