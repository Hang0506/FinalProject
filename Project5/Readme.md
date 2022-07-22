   
        "BucketPolicy": {
            "Type": "AWS::S3::BucketPolicy",
            "Properties": {
                "Bucket": "!Ref AttachmentsBucket",
                "PolicyDocument": {
                    "Id": "MyPolicy",
                    "Version": "2012-10-17",
                    "Statement": {
                        "Effect": "Allow",
                        "Principal": "*",
                        "Action": "*",
                        "Resource": {
                            "Fn::Join": [
                                "",
                                [
                                    "arn:aws:s3:::",
                                    {
                                        "Ref": "AttachmentsBucket"
                                    },
                                    "/*"
                                ]
                            ]
                        }
                    }
                }
            }
        }