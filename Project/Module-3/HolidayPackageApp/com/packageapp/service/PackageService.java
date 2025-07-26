package com.packageapp.service;

import java.util.List;
import com.packageapp.model.Package;
import com.packageapp.exception.InvalidPackageIdException;

public interface PackageService {
    void addPackage(Package pkg) throws InvalidPackageIdException;
    List<Package> fetchAllPackages();
    Package findPackageById(String id);
    void calculateCost(String id) throws InvalidPackageIdException;
}