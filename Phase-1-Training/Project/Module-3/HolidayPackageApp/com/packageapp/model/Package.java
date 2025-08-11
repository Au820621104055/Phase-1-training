package com.packageapp.model;

public class Package {
    private String packageId;
    private String sourcePlace;
    private String destinationPlace;
    private int noOfDays;
    private double basicFare;
    private double packageCost;

    public Package() {}

    public Package(String packageId, String sourcePlace, String destinationPlace, int noOfDays, double basicFare) {
        this.packageId = packageId;
        this.sourcePlace = sourcePlace;
        this.destinationPlace = destinationPlace;
        this.noOfDays = noOfDays;
        this.basicFare = basicFare;
    }

    

    public String getPackageId() {
		return packageId;
	}

	public void setPackageId(String packageId) {
		this.packageId = packageId;
	}

	public String getSourcePlace() {
		return sourcePlace;
	}

	public void setSourcePlace(String sourcePlace) {
		this.sourcePlace = sourcePlace;
	}

	public String getDestinationPlace() {
		return destinationPlace;
	}

	public void setDestinationPlace(String destinationPlace) {
		this.destinationPlace = destinationPlace;
	}

	public int getNoOfDays() {
		return noOfDays;
	}

	public void setNoOfDays(int noOfDays) {
		this.noOfDays = noOfDays;
	}

	public double getBasicFare() {
		return basicFare;
	}

	public void setBasicFare(double basicFare) {
		this.basicFare = basicFare;
	}

	public double getPackageCost() {
		return packageCost;
	}

	public void setPackageCost(double packageCost) {
		this.packageCost = packageCost;
	}

	@Override
    public String toString() {
        return "Package ID: " + packageId + ", Source: " + sourcePlace + ", Destination: " + destinationPlace +
               ", Days: " + noOfDays + ", Basic Fare: " + basicFare + ", Package Cost: " + packageCost;
    }
}