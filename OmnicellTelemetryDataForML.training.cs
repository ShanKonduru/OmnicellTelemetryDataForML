﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

namespace OmnicellTelemetryDataForML.ConsoleApp
{
    public partial class OmnicellTelemetryDataForML
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(new []{new InputOutputColumnPair(@"Client Name", @"Client Name"),new InputOutputColumnPair(@"Hardware Item", @"Hardware Item"),new InputOutputColumnPair(@"Cabinet Address", @"Cabinet Address"),new InputOutputColumnPair(@"Operation", @"Operation")}, outputKind: OneHotEncodingEstimator.OutputKind.Indicator)      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"Duration Mins", @"Duration Mins"),new InputOutputColumnPair(@"Duration Secs", @"Duration Secs"),new InputOutputColumnPair(@"Duration in Secs", @"Duration in Secs")}))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Open Date & Time",outputColumnName:@"Open Date & Time"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"Close Date & Time",outputColumnName:@"Close Date & Time"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"Client Name",@"Hardware Item",@"Cabinet Address",@"Operation",@"Duration Mins",@"Duration Secs",@"Duration in Secs",@"Open Date & Time",@"Close Date & Time"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName:@"Status",inputColumnName:@"Status"))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(new SdcaMaximumEntropyMulticlassTrainer.Options(){L1Regularization=1F,L2Regularization=0.1F,LabelColumnName=@"Status",FeatureColumnName=@"Features"}))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName:@"PredictedLabel",inputColumnName:@"PredictedLabel"));

            return pipeline;
        }
    }
}
